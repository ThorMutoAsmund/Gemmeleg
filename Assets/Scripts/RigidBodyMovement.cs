using UnityEngine;

namespace Gemmeleg
{
    public class RigidBodyMovement : MonoBehaviour
    {
        private readonly float rotationSpeed = 3f;
        private readonly float lookSpeed = 3f;
        private readonly float lookDownMax = 90f - 10f;
        private readonly float lookUpMax = 270f + 10f;

        private readonly float maxBackwardsVelocity = 15f;
        private readonly float maxBackwardsRunVelocity = 50f;
        private readonly float backwardsForce = 2500f;
        private readonly float backwardsRunForce = 5000f;

        private readonly float maxForwardVelocity = 15f;
        private readonly float maxForwardRunVelocity = 50f;
        private readonly float forwardForce = 2500f;
        private readonly float forwardRunForce = 5000f;

        private readonly float maxSidewaysVelocity = 10f;
        private readonly float sidewaysForce = 2000f;

        private readonly float jumpForce = 500f;

        private readonly float deceleration = 400f;
        private readonly int countToUngrounded = 3;

        private Rigidbody body;
        private bool isGrounded = true;
        private int groundCounter;
        private new Camera camera;

        private void Start()
        {
            this.body = GetComponent<Rigidbody>();
            this.body.drag = 0;
            this.camera = this.GetComponentInChildren<Camera>();
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
                if (localVel.z < (shiftIsDown ? this.maxForwardRunVelocity : this.maxForwardVelocity))
                {
                    this.body.AddForce(forwardVector * (shiftIsDown ? this.forwardRunForce : this.forwardForce));
                }

            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                if (localVel.z > (shiftIsDown ? -this.maxBackwardsRunVelocity : -this.maxBackwardsVelocity))
                {
                    this.body.AddForce(backVector * (shiftIsDown ? this.backwardsRunForce : this.backwardsForce));
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

            if (this.isGrounded)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    this.isGrounded = false;
                    this.groundCounter = 0;
                    this.body.AddForce(new Vector3(0, this.jumpForce, 0), ForceMode.Impulse);
                }
            }
            else
            {
                if (localVel.y < 0.1f && localVel.y > -0.1f)
                {
                    groundCounter++;
                    if (groundCounter >= this.countToUngrounded)
                    {
                        this.isGrounded = true;
                    }
                }
                else
                {
                    this.groundCounter = 0;
                }
            }
        }

        private void Update()
        {
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
