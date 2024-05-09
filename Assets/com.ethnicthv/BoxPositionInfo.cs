using System;
using UnityEngine;

namespace com.ethnicthv
{
    public struct BoxPositionInfo
    {
        public Vector3 Position;
        public Vector3 VelocityVector;
        public float Velocity { get; private set; }
        
        public BoxPositionInfo(Vector3 position)
        {
            Position = position;
            VelocityVector = Vector3.up;
            Velocity = 0.0f;
        }
        
        public void MoveWithVelocity(float velocity, int direction)
        {
            Velocity = velocity;
            VelocityVector = direction switch
            {
                0 => Vector3.up,
                1 => Vector3.down,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public bool UpdatePosition()
        {
            //<<--update the position based on the velocity-->>
            var temp = VelocityVector * (Velocity / UsefulConstant.UnitPerMeter) + Position;
            //check if the position is within the bounds of the box
            if (temp.y is > UsefulConstant.UpperBound or < UsefulConstant.LowerBound)
            {
                temp.y = Mathf.Clamp(temp.y, -1.0f, 1.0f);
                //change the direction of the velocity
                Velocity = -Velocity;
            }
            else
            {
                Position = temp;
            }
            // Debug.Log($"Old Position: {_oldPosition}");
            // Debug.Log($"Position: {Position}");
            // Debug.Log("temp: " + temp);
            // Debug.Log("Velocity: " + Velocity);
            
            //return false to indicate that the box is still moving, keep it in the dirty list
            return false;
        }
    }
}