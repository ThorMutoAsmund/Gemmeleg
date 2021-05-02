using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gemmeleg
{
    public class PlayerInteraction : MonoBehaviour
    {
        private readonly float interactionRadius = 2f;
        private Collider[] colliders;

        private void Awake()
        {
            this.colliders = new Collider[10];
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                AttemptInteractionWithE();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                AttemptInteractionWithF();
            }
        }

        private void AttemptInteractionWithE()
        {
            var numberOfColliders = Physics.OverlapSphereNonAlloc(this.transform.position, this.interactionRadius, this.colliders, Layers.InteractionLayerMask);
            var minDist = float.MaxValue;
            Collider selectedCollider = null;
            for (int i = 0; i < numberOfColliders; ++i)
            {
                var dist = Vector3.Distance(this.colliders[i].transform.position, this.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    selectedCollider = this.colliders[i];
                }
            }

            if (selectedCollider != null)
            {
                if (selectedCollider.TryGetComponent<ICanOpen>(out var canOpen))
                {
                    canOpen.Toogle();
                }
                else if (selectedCollider.TryGetComponent<ICanBeTaken>(out var canBeTaken))
                {
                    canBeTaken.Take(this.transform);
                }
            }
        }

        private void AttemptInteractionWithF()
        {
            var numberOfColliders = Physics.OverlapSphereNonAlloc(this.transform.position, this.interactionRadius, this.colliders, Layers.InteractionLayerMask);
            var minDist = float.MaxValue;
            Collider selectedCollider = null;
            for (int i = 0; i < numberOfColliders; ++i)
            {
                var dist = Vector3.Distance(this.colliders[i].transform.position, this.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    selectedCollider = this.colliders[i];
                }
            }

            if (selectedCollider != null)
            {
                ICanBeDriven canBeDriven;
                if (selectedCollider.TryGetComponent<ICanBeDriven>(out canBeDriven) || (selectedCollider.transform.parent != null && selectedCollider.transform.parent.TryGetComponent<ICanBeDriven>(out canBeDriven)))
                {
                    canBeDriven.Enter(this.gameObject);
                }
            }
        }
    }
}
