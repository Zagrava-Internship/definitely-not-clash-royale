using Combat;
using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "UnitConfig", menuName = "Game/UnitConfig")]
    public class UnitConfig : ScriptableObject
    {
        public string unitName;
        public GameObject prefab;
        public GameObject ghostPrefab;
        public int health;
        public float speed;
        //public int damage; // This is now part of the weapon data
        //public UnitType type; // This is now part of the weapon data
        public WeaponBase weaponData; // Reference to the weapon data for this unit
        public float aggressionRange = 5f; // Range at which the unit will start attacking enemies
    }
}