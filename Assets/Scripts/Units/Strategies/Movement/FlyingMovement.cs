using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Strategies.Movement
{
    public class FlyingMovement : MonoBehaviour, IMovementStrategy
    {
        private const float ArriveEpsilon = 0.05f;
        private Coroutine _flyRoutine;

        public void Move(Unit unit, Vector3 destination, float speed)
        {
            if (_flyRoutine != null) StopCoroutine(_flyRoutine);
            _flyRoutine = StartCoroutine(Fly(unit, destination, speed));
        }

        public void Stop(Unit unit)
        {
            if (_flyRoutine != null) StopCoroutine(_flyRoutine);
            _flyRoutine = null;
        }

        private IEnumerator Fly(Unit unit, Vector3 destination, float speed)
        {
            destination.z = unit.transform.position.z;

            while (Vector3.Distance(unit.transform.position, destination) > ArriveEpsilon)
            {
                var dir = (destination - unit.transform.position).normalized;
                // Ensure Z position is constant for flying units
                dir.z = 0; // Keep the Z component zero for horizontal movement
                unit.transform.position += dir * speed * Time.deltaTime;
                unit.RotateFromDirection(dir);
                // 

                unit.Animator.ChangeMovingDirection(new Vector2(dir.x, dir.y));

                yield return null;
            }

            _flyRoutine = null;
        }
    }

}