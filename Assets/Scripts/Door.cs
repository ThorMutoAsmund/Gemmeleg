using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gemmeleg
{
    public class Door : MonoBehaviour, ICanOpen
    {
        [SerializeField] private Vector3 closedRotation = Vector3.zero;
        [SerializeField] private Vector3 openRotation = Vector3.zero;
        [SerializeField] private GameObject blockingObject = null;
        [SerializeField] private AudioSource openSound;
        [SerializeField] private AudioSource closeSound;

        private float openCloseDuration = 0.3f;
        private bool isOpen;
        private Coroutine openingCoroutine;
        private bool isMoving => this.openingCoroutine != null;
        private AudioSource audioSource;

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private IEnumerator PerformOpening(Quaternion endRotation)
        {
            var startRotation = this.transform.localRotation;
            var t = 0f;
            while (t < this.openCloseDuration)
            {
                this.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t / this.openCloseDuration);
                t += Time.deltaTime;
                yield return null;
            }
            this.transform.localRotation = endRotation;

            this.openingCoroutine = null;            
        }

        public void Toogle()
        {
            if (!this.isMoving)
            {
                this.isOpen = !this.isOpen;
                this.blockingObject.SetActive(!this.isOpen);
                this.openingCoroutine = StartCoroutine(PerformOpening(Quaternion.Euler(this.isOpen ? this.openRotation : this.closedRotation)));

                if (this.isOpen && this.openSound != null)
                {
                    this.openSound.Play();
                }
                else if (!this.isOpen && this.closeSound != null)
                {
                    this.closeSound.Play();
                }
            }
        }
    }
}
