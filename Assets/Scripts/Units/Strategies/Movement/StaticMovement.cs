using UnityEngine;

namespace Units.Strategies.Movement
{
    public class StaticMovement: MonoBehaviour, IMovementStrategy
    {
        public void Move(Unit unit, Vector3 destination, float speed)
        {
            // Static movement does not change position, so we do nothing here.
            // This is useful for units that do not move
        }

        public void Stop(Unit unit)
        {
            // Static movement does not require stopping, so we do nothing here as well.
            // This is useful for units that do not move    
        }
    }
}