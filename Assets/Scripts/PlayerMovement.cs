using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Gemmeleg
{
    public class PlayerMovement : MonoBehaviour
    {
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

        private readonly float jumpForce = 800f;

        private readonly float deceleration = 400f;

        private Rigidbody body;
        private new Camera camera;
        private float distToGround;
        private NavMeshAgent navAgent;

        private void Start()
        {
            this.body = GetComponent<Rigidbody>();
            this.body.drag = 0;
            this.camera = this.GetComponentInChildren<Camera>();
            this.distToGround = GetComponent<SphereCollider>().radius;
            this.navAgent = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
            var localVel = this.transform.InverseTransformDirection(this.body.velocity);

            var forwardVector = this.transform.forward;
            var backVector = -this.transform.forward;
            var decelerationVector = -this.body.velocity;
            decelerationVector.y = 0;

            this.body.AddForce(decelerationVector * this.deceleration);

            var shiftIsDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                if (localVel.z < (!shiftIsDown ? this.maxForwardWalkVelocity : this.maxForwardSneakVelocity))
                {
                    this.body.AddForce(forwardVector * (!shiftIsDown ? this.forwardWalkForce : this.forwardSneakForce));
                }

            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                if (localVel.z > (!shiftIsDown ? -this.maxBackwardsWalkVelocity : -this.maxBackwardsSneakVelocity))
                {
                    this.body.AddForce(backVector * (!shiftIsDown ? this.backwardsWalkForce : this.backwardsSneakForce));
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                if (localVel.x > -this.maxSidewaysVelocity)
                {
                    this.body.AddForce(this.transform.right * -this.sidewaysForce);
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                if (localVel.x < this.maxSidewaysVelocity)
                {
                    this.body.AddForce(this.transform.right * this.sidewaysForce);
                }
            }
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, this.distToGround + 1.0f);
        }

        private void Update()
        {
            if (IsGrounded())
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F))
                {
                    this.body.AddForce(new Vector3(0, this.jumpForce, 0), ForceMode.Impulse);
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
    }
}