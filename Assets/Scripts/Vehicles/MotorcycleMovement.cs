using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Gemmeleg
{
    public class MotorcycleMovement : VehicleMovement
    {
        protected override float maxBackwardsDriveVelocity => 15f;
        protected override float backwardsDriveForce => 4500f;
        protected override float maxForwardDriveVelocity => 30f;
        protected override float forwardDriveForce => 9000f;
        protected override float deceleration => 40f;
        protected override float rotationSpeed => 0.125f;

    }
}