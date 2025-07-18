using UnityEngine;

namespace Units.Strategies.Movement
{
    public class StaticMovement: MonoBehaviour, IMovementStrategy
    {
        public void Move(Unit unit, Vector3 destination, float speed)
        {
            // TODO
            throw new System.NotImplementedException();
        }

        public void Stop(Unit unit)
        {
            // TODO
            throw new System.NotImplementedException();
        }
    }
}