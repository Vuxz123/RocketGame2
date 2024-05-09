using Unity.VisualScripting;
using UnityEngine;

namespace com.ethnicthv
{
    public class BoxPositionInfo
    {
        private Vector3 _origin;
        private int _numOfTimeSteps;
        
        public Vector3 Position;
        public Vector3 VelocityVector = Vector3.up;
        public float Velocity { get; private set; }
        public float Acceleration { get; private set; }
        
        public BoxPositionInfo(Vector3 position)
        {
            _origin = position;
        }
        
        public void Accelerate(float acceleration, int numOfTimeSteps = 1)
        {
            Acceleration = acceleration;
            _numOfTimeSteps = numOfTimeSteps;
        }
        
        public void MoveWithVelocity(float velocity)
        {
            Velocity = velocity;
        }

        public void UpdatePosition(float deltaTime)
        {
            //<<__update the velocity based on the acceleration-->>
            if (_numOfTimeSteps > 0)
            {
                Velocity += Acceleration * deltaTime;
                _numOfTimeSteps--;
            }
            else
            {
                //set the acceleration to a negative value to simulate deceleration
                Acceleration = -0.1f;
            }
            
            //<<--update the position based on the velocity-->>
            var temp = VelocityVector * (Velocity * deltaTime / UsefulConstant.UnitPerMeter) + Position;
            //Check if the position is within the bounds of the box
            if (temp.y is > 1.0f or < -1.0f)
            {
                temp.y = Mathf.Clamp(temp.y, -1.0f, 1.0f);
                //change the direction of the velocity
                Velocity = -Velocity;
            }
            else
            {
                Position = temp;
            }
        }
    }
}