using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Game/UnitData")]
    public class UnitData : ScriptableObject
    {
        public string unitName;
        public GameObject prefab;
        public GameObject ghostPrefab;
        public float health;
        public float speed;
        public float damage;
        public UnitType type;
    }

    public enum UnitType
    {
        Melee,
        Ranged,
        Flying
    }
}