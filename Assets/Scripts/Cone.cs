using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gemmeleg
{
    public class Cone : MonoBehaviour, ICanBeTaken
    {
        private float flyDuration = 0.15f;

        private IEnumerator PerformTaking(Transform endTransform)
        {
            this.transform.parent = null;
            var startPosition = this.transform.position;
            var t = 0f;
            while (t < this.flyDuration)
            {
                this.transform.position = Vector3.Lerp(startPosition, endTransform.position, t / this.flyDuration);
                t += Time.deltaTime;
                yield return null;
            }
            Hide(true);
        }

        public ICanBeTaken Take(Transform actor)
        {
            var rigidBody = this.GetComponentInChildren<Rigidbody>();
            rigidBody.isKinematic = true;

            StartCoroutine(PerformTaking(actor));
            //if (this.takeSound != null)
            //{
            //    this.takeSound.Play();
            //}

            return this;
        }

        private void Hide(bool doHide)
        {
            foreach (var r in this.GetComponentsInChildren<Renderer>())
            {
                r.enabled = !doHide;
            }
            foreach (var r in this.GetComponentsInChildren<Collider>())
            {
                r.enabled = !doHide;
            }
        }
        public void Drop(GameObject player)
        {
            this.transform.rotation = Quaternion.Euler(0f, this.transform.rotation.eulerAngles.y, 0f);
            this.transform.position = player.transform.position + player.transform.forward * 2f;

            var rigidBody = this.GetComponentInChildren<Rigidbody>();
            rigidBody.isKinematic = false;

            Hide(false);
        }
    }
}
