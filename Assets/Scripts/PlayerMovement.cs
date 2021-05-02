using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Gemmeleg
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private AudioSource runningAudioSource;
        [SerializeField] private AudioSource jumpAudioSource;
        [SerializeField] private AudioSource walkingAudioSource;
        [SerializeField] private Collider normalCollider;
        [SerializeField] private Collider crouchCollider;
        [SerializeField] private Transform cameraOffset;

        public Transform CameraOffset => this.cameraOffset;
        public Camera Camera => this.camera;

        private readonly float rotationSpeed = 1.5f;
        private readonly float lookSpeed = 1.2f;
        private readonly float lookDownMax = 90f - 10f;
        private readonly float lookUpMax = 270f + 10f;

        private readonly float maxBackwardsSneakVelocity = 15f;
        private readonly float maxBackwardsWalkVelocity = 50f;
        private readonly float backwardsSneakForce = 2500f;
        private readonly float backwardsWalkForce = 7500f;

        private readonly float maxForwardSneakVelocity = 15f;
        private readonly float maxForwardWalkVelocity = 50f;
        private readonly float forwardSneakForce = 2500f;
        private readonly float forwardWalkForce = 7500f;

        private readonly float maxSidewaysVelocity = 20f;
        private readonly float sidewaysForce = 4000f;

        private readonly float jumpForce = 1200f;
        private readonly float crouchTime = 0.15f;

        private readonly float deceleration = 400f;

        private Rigidbody body;
        private new Camera camera;
        private float distToGround;
        private NavMeshAgent navAgent;
        private Vector3 cameraNormalPosition;
        private Vector3 cameraCrouchPosition;
        private bool isCrouching;
        private Coroutine crouchCoroutine;

        private void Start()
        {
            this.body = GetComponent<Rigidbody>();
            this.body.drag = 0;
            this.camera = this.GetComponentInChildren<Camera>();
            this.distToGround = GetComponent<SphereCollider>().radius;
            this.navAgent = GetComponent<NavMeshAgent>();
            this.cameraNormalPosition = new Vector3(0f, 1.5f, 0f);
            this.cameraCrouchPosition = new Vector3(0f, 1.5f*0.6f, 0f);
        }

        private void FixedUpdate()
        {
            var localVel = this.transform.InverseTransformDirection(this.body.velocity);

            var forwardVector = this.transform.forward.With(y: -0.35f);
            var backVector = (-this.transform.forward).With(y: -0.35f);
            var decelerationVector = -this.body.velocity;
            decelerationVector.y = 0;

            this.body.AddForce(decelerationVector * this.deceleration);

            var shiftIsDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            var ctrlIsDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            var useSneakSpeed = shiftIsDown || ctrlIsDown;
            bool playRunningSound = false;

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                if (localVel.z < (useSneakSpeed ? this.maxForwardSneakVelocity : this.maxForwardWalkVelocity))
                {
                    this.body.AddForce(forwardVector * (useSneakSpeed ? this.forwardSneakForce : this.forwardWalkForce));
                }
                playRunningSound = true;
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                if (localVel.z > (useSneakSpeed ? -this.maxBackwardsSneakVelocity : -this.maxBackwardsWalkVelocity))
                {
                    this.body.AddForce(backVector * (useSneakSpeed ? this.backwardsSneakForce : this.backwardsWalkForce));
                }
                playRunningSound = true;
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                if (localVel.x > -this.maxSidewaysVelocity)
                {
                    this.body.AddForce(this.transform.right * -this.sidewaysForce);
                }
                playRunningSound = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                if (localVel.x < this.maxSidewaysVelocity)
                {
                    this.body.AddForce(this.transform.right * this.sidewaysForce);
                }
                playRunningSound = true;
            }

            if (ctrlIsDown != this.isCrouching)
            {
                this.isCrouching = ctrlIsDown;
                this.normalCollider.enabled = !this.isCrouching;
                this.crouchCollider.enabled = this.isCrouching;
                if (this.crouchCoroutine != null)
                {
                    StopCoroutine(this.crouchCoroutine);
                }
                this.crouchCoroutine = StartCoroutine(Crouch(this.isCrouching));
            }

            if (playRunningSound && !shiftIsDown)
            {
                if (!this.runningAudioSource.isPlaying)
                {
                    this.runningAudioSource.Play();
                }
                this.walkingAudioSource.Stop();
            }
            else if (playRunningSound && shiftIsDown)
            {
                if (!this.walkingAudioSource.isPlaying)
                {
                    this.walkingAudioSource.Play();
                }
                this.runningAudioSource.Stop();
            }
            else
            {
                this.walkingAudioSource.Stop();
                this.runningAudioSource.Stop();
            }
        }

        private void Update()
        {
            if (IsGrounded())
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    this.body.AddForce(new Vector3(0, this.jumpForce, 0), ForceMode.Impulse);
                    this.jumpAudioSource.Play();
                }
            }
            else
            {
                this.body.AddForce(Physics.gravity, ForceMode.Acceleration);
            }

            float mouse = Input.GetAxis("Mouse X");
            if (mouse != 0f)
            {
                this.transform.Rotate(this.transform.up, mouse * this.rotationSpeed);
            }

            mouse = Input.GetAxis("Mouse Y");
            if (mouse != 0f)
            {
                var euler = new Vector3(this.camera.transform.localRotation.eulerAngles.x - mouse * this.lookSpeed, 0f, 0f);
                if (euler.x < this.lookUpMax && euler.x > 180f)
                {
                    euler.x = this.lookUpMax;
                }
                else if (euler.x > this.lookDownMax && euler.x < 180f)
                {
                    euler.x = this.lookDownMax;
                }

                this.camera.transform.localRotation = Quaternion.Euler(euler);
            }
        }

        public void ResetCamera(Vector3 position)
        {
            this.camera.transform.parent = this.cameraOffset.transform;
            this.camera.transform.localPosition = Vector3.zero;
            this.camera.transform.localRotation = Quaternion.identity;
            this.transform.position = position;
        }

        private IEnumerator Crouch(bool crouch)
        {
            var duration = 0f;
            var start = this.cameraOffset.transform.localPosition;
            var end = crouch ? this.cameraCrouchPosition : this.cameraNormalPosition;
            while (duration < this.crouchTime)
            {
                this.cameraOffset.transform.localPosition = Vector3.Lerp(start, end, duration / this.crouchTime);
                duration += Time.deltaTime;
                yield return null;
            }
            this.cameraOffset.transform.localPosition = end;
            this.crouchCoroutine = null;
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, this.distToGround + 0.8f);
        }

    }
}