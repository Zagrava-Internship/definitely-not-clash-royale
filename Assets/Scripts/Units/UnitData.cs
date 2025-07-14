using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Game/UnitData")]
    public class UnitData : ScriptableObject
    {
        public string unitName;
        public GameObject prefab;
        public GameObject ghostPrefab;
        public int health;
        public float speed;
        public int damage;
        public UnitType type;
    }

    public enum UnitType
    {
        Melee,
        Ranged,
        Flying
    }
}