using Maps.MapManagement.Grid;
using UnityEngine;

namespace Cards
{
    public static class DropValidator
    {
        public static bool IsValidDropPosition(Vector3 worldPosition)
        {
            var node = GridManager.Instance.GetNodeFromWorldPoint(worldPosition);
            return node is { IsWalkable: true };
        }
    }
}