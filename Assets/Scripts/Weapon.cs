using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gemmeleg
{
    public class Weapon : MonoBehaviour, ICanBeTaken
    {
        [SerializeField] private AudioSource takeSound;

        private float flyDuration = 0.3f;

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

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
            Hide();
        }
        
        public void Take(Transform actor)
        {
            StartCoroutine(PerformTaking(actor));
            if (this.takeSound != null)
            {
                this.takeSound.Play();
            }
        }

        private  void Hide()
        {
            foreach (var r in this.GetComponentsInChildren<Renderer>())
            {
                r.enabled = false;
            }
            foreach (var r in this.GetComponentsInChildren<Collider>())
            {
                r.enabled = false;
            }
        }
    }
}
