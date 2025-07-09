using UnityEngine;

namespace Maps.MapManagement.Grid.Strategies
{
    public abstract class RegionStrategy:ScriptableObject
    {
        [SerializeField] protected Vector2Int center;
        [SerializeField] protected Vector2Int size;
        public abstract bool IsOccupied(Vector2Int pos);
    }
}