using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gemmeleg
{
    public class Vehicle : MonoBehaviour, ICanBeDriven
    {
        private GameObject playerGameObject;
        private Vector3 offsetWhenEntered;

        public void Enter(GameObject playerGameObject)
        {
            this.playerGameObject = playerGameObject;
            this.offsetWhenEntered = this.playerGameObject.transform.position - this.transform.position;

            var playerMovement = playerGameObject.GetComponent<PlayerMovement>();
            playerMovement.enabled = false;

            var vehicleMovement = GetComponent<VehicleMovement>();
            vehicleMovement.SetCamera(playerMovement.Camera);
            vehicleMovement.enabled = true;

            var rigidBody = GetComponent<Rigidbody>();
            rigidBody.freezeRotation = true;
            this.transform.rotation = Quaternion.Euler(0f, this.transform.rotation.eulerAngles.y, 0f);
        }

        public void Exit()
        {
            var playerMovement = playerGameObject.GetComponent<PlayerMovement>();
            playerMovement.enabled = true;

            var playerRigidBody = playerGameObject.GetComponent<Rigidbody>();
            playerRigidBody.isKinematic = false;

            var vehicleMovement = GetComponent<VehicleMovement>();
            vehicleMovement.enabled = false;

            playerMovement.ResetCamera(this.transform.position + this.offsetWhenEntered);
            
            this.playerGameObject = null;

            var rigidBody = GetComponent<Rigidbody>();
            rigidBody.freezeRotation = false;
        }
    }
}
