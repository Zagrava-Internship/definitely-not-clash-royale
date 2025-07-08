using Grid;
using UnityEngine;

namespace Cards
{
    public static class DropValidator
    {
        public static bool IsValidDropPosition(Vector3 worldPosition)
        {
            var node = GridManager.Instance.GetClosestNode(worldPosition);
            return node is { isWalkable: true };
        }
    }
}