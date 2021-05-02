using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Gemmeleg
{
    public class CarMovement : MonoBehaviour
    {
        private readonly float maxBackwardsWalkVelocity = 200f;
        private readonly float backwardsWalkForce = 60000f;

        private readonly float maxForwardWalkVelocity = 200f;
        private readonly float forwardWalkForce = 60000f;

        private readonly float maxRotateVelocity = 200f;
        private readonly float rotateForce = 30000f;

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

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                if (localVel.z < this.maxForwardWalkVelocity)
                {
                    this.body.AddForce(forwardVector * this.forwardWalkForce);
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                if (localVel.z > -this.maxBackwardsWalkVelocity)
                {
                    this.body.AddForce(backVector * this.backwardsWalkForce);
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                this.transform.Rotate(0f, -1f, 0f);
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                this.transform.Rotate(0f, 1f, 0f);
            }
        }


        private void Update()
        {
        }
    }
}