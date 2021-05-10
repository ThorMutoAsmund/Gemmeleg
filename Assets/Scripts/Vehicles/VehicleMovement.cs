using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Gemmeleg
{
    public abstract class VehicleMovement : MonoBehaviour
    {
        [SerializeField] private Transform cameraOffset;

        protected abstract float maxBackwardsDriveVelocity { get; }
        protected abstract float backwardsDriveForce { get; }
        protected abstract float maxForwardDriveVelocity { get; }
        protected abstract float forwardDriveForce { get; }
        protected abstract float deceleration { get; }
        protected abstract float rotationSpeed { get; }

        private readonly float cameraRotationSpeed = 1.5f;
        private readonly float cameraLookSpeed = 1.2f;
        private readonly float cameraLookUpMax = 270f + 10f;
        private readonly float cameraLookDownMax = 90f - 10f;

        private Rigidbody body;
        private new Camera camera;
        private float rotation;

        private void Start()
        {
            this.body = GetComponent<Rigidbody>();
            this.body.drag = 0;
        }

        private void FixedUpdate()
        {
            var localVel = this.transform.InverseTransformDirection(this.body.velocity);

            var forwardVector = this.transform.forward.With(y: -0.35f);
            var backVector = (-this.transform.forward).With(y: -0.35f);
            var decelerationVector = -this.body.velocity;
            decelerationVector.y = 0;

            this.body.AddForce(decelerationVector * this.deceleration);

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                if (localVel.z < this.maxForwardDriveVelocity)
                {
                    this.body.AddForce(forwardVector * this.forwardDriveForce);
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                if (localVel.z > -this.maxBackwardsDriveVelocity)
                {
                    this.body.AddForce(backVector * this.backwardsDriveForce);
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                this.transform.Rotate(this.transform.up, -localVel.magnitude * (localVel.z > 0 ? this.rotationSpeed : -this.rotationSpeed));
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                this.transform.Rotate(this.transform.up, localVel.magnitude * (localVel.z > 0 ? this.rotationSpeed : -this.rotationSpeed));
            }
        }

        private void Update()
        {
            float mouse = Input.GetAxis("Mouse X");
            if (mouse != 0f)
            {
                this.cameraOffset.Rotate(this.transform.up, mouse * this.cameraRotationSpeed);
            }

            mouse = Input.GetAxis("Mouse Y");
            if (mouse != 0f)
            {
                var euler = new Vector3(this.camera.transform.localRotation.eulerAngles.x - mouse * this.cameraLookSpeed, 0f, 0f);
                if (euler.x < this.cameraLookUpMax && euler.x > 180f)
                {
                    euler.x = this.cameraLookUpMax;
                }
                else if (euler.x > this.cameraLookDownMax && euler.x < 180f)
                {
                    euler.x = this.cameraLookDownMax;
                }

                this.camera.transform.localRotation = Quaternion.Euler(euler);
            }
        }

        public void SetCamera(Camera camera)
        {
            this.camera = camera;
            this.camera.transform.parent = this.cameraOffset;
            this.camera.transform.localPosition = Vector3.zero;
            this.camera.transform.localRotation = Quaternion.identity;
            this.transform.localRotation = Quaternion.identity;
        }
    }
}