using Combat;
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
        //public int damage; // This is now part of the weapon data
        //public UnitType type; // This is now part of the weapon data
        public WeaponBase weaponData; // Reference to the weapon data for this unit
    }
}