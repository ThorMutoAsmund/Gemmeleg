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
        private ICanBeDriven vehicle;
        private ICanBeTaken holding;
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
            if (this.holding != null)
            {
                this.holding.Drop(this.gameObject);
                this.holding = null;
                return;
            }

            var numberOfColliders = Physics.OverlapSphereNonAlloc(this.transform.position, this.interactionRadius, this.colliders, Layers.InteractionLayerMask);
            var minDist = float.MaxValue;
            IEInteractive selectedInteractive = null;
            for (int i = 0; i < numberOfColliders; ++i)
            {
                var interactive = this.colliders[i].GetComponentInParent<IEInteractive>();
                if (interactive != null)
                {
                    var dist = Vector3.Distance(this.colliders[i].transform.position, this.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        selectedInteractive = interactive;
                    }
                }
            }

            if (selectedInteractive != null)
            {
                if (selectedInteractive is ICanOpen canOpen)
                {
                    canOpen.Toogle();
                }
                else if (selectedInteractive is ICanBeTaken canBeTaken)
                {
                    this.holding = canBeTaken.Take(this.transform);
                }
            }
        }

        private void AttemptInteractionWithF()
        {
            // Get off?
            if (this.vehicle != null)
            {
                this.vehicle.Exit();
                this.vehicle = null;
                return;
            }

            // Get on?
            var numberOfColliders = Physics.OverlapSphereNonAlloc(this.transform.position, this.interactionRadius, this.colliders, Layers.InteractionLayerMask);
            var minDist = float.MaxValue;
            IFInteractive selectedInteractive = null;
            for (int i = 0; i < numberOfColliders; ++i)
            {
                var interactive = this.colliders[i].GetComponentInParent<IFInteractive>();
                if (interactive != null)
                {
                    var dist = Vector3.Distance(this.colliders[i].transform.position, this.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        selectedInteractive = interactive;
                    }
                }
            }

            if (selectedInteractive != null)
            {
                if (selectedInteractive is ICanBeDriven vehicle)
                {
                    this.vehicle = vehicle;
                    this.vehicle.Enter(this.gameObject);
                }
            }
        }
    }
}
