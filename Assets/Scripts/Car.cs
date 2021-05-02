using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gemmeleg
{
    public class Car : MonoBehaviour, ICanBeDriven
    {
        [SerializeField] private Transform passengerTransform;

        private GameObject playerGameObject;
        private Vector3 offsetWhenEntered;
        public void Enter(GameObject playerGameObject)
        {
            this.playerGameObject = playerGameObject;
            this.offsetWhenEntered = this.playerGameObject.transform.position - this.transform.position;

            var playerMovement = playerGameObject.GetComponent<PlayerMovement>();
            playerMovement.MovementEnabled = false;

            //var playerRigidBody = playerGameObject.GetComponent<Rigidbody>();
            //playerRigidBody.isKinematic = true;
            //playerRigidBody.dis

            var carMovement = GetComponent<CarMovement>();
            carMovement.enabled = true;

            Camera.main.transform.parent = this.transform;
            Camera.main.transform.localPosition = this.passengerTransform.localPosition;
        }

        public void Exit()
        {
            var playerMovement = playerGameObject.GetComponent<PlayerMovement>();
            playerMovement.MovementEnabled = true;

            var playerRigidBody = playerGameObject.GetComponent<Rigidbody>();
            playerRigidBody.isKinematic = false;

            var carMovement = GetComponent<CarMovement>();
            carMovement.enabled = false;

            Camera.main.transform.parent = playerMovement.CameraOffset.transform;
            Camera.main.transform.localPosition = this.passengerTransform.localPosition;

            this.playerGameObject = null;
        }
    }
}
