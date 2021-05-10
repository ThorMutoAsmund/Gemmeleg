using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Gemmeleg
{
    public class CarMovement : VehicleMovement
    {
        protected override float maxBackwardsDriveVelocity => 150f;
        protected override float backwardsDriveForce => 45000f;
        protected override float maxForwardDriveVelocity => 300f;
        protected override float forwardDriveForce => 90000f;
        protected override float deceleration => 400f;
        protected override float rotationSpeed => 0.025f;
    }
}