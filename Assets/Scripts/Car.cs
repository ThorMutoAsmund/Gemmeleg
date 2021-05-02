using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gemmeleg
{
    public class Car : MonoBehaviour, ICanBeDriven
    {
        private GameObject playerGameObject;
        private Vector3 offsetWhenEntered;

        public void Enter(GameObject playerGameObject)
        {
            this.playerGameObject = playerGameObject;
            this.offsetWhenEntered = this.playerGameObject.transform.position - this.transform.position;

            var playerMovement = playerGameObject.GetComponent<PlayerMovement>();
            playerMovement.enabled = false;

            var carMovement = GetComponent<CarMovement>();
            carMovement.SetCamera(playerMovement.Camera);
            carMovement.enabled = true;
        }

        public void Exit()
        {
            var playerMovement = playerGameObject.GetComponent<PlayerMovement>();
            playerMovement.enabled = true;

            var playerRigidBody = playerGameObject.GetComponent<Rigidbody>();
            playerRigidBody.isKinematic = false;

            var carMovement = GetComponent<CarMovement>();
            carMovement.enabled = false;

            playerMovement.ResetCamera(this.transform.position + this.offsetWhenEntered);
            
            this.playerGameObject = null;
        }
    }
}
