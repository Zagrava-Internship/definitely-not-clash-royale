using UnityEngine;

namespace Units.Strategies
{
    public interface IMovementStrategy
    {
        void Move(Unit unit, Vector3 destination, float speed);
        void Stop(Unit unit);
    }
}