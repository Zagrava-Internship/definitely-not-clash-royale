using System.Linq;
using UnityEngine;

namespace Units
{
    public static class TargetFinder
    {
        public static ITargetable FindClosestTarget(Vector3 position)
        {
            var allTargets = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<ITargetable>()
                .Where(t => !t.IsDead)
                .ToList();

            if (allTargets.Count == 0)
                return null;


            return allTargets
                .OrderBy(t => Vector2.SqrMagnitude(new Vector2(position.x, position.y) - new Vector2(t.Position.x, t.Position.y)))
                .First();
        }

    }
}