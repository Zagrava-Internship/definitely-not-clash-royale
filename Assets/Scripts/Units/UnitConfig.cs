using Combat;
using Units.Enums;
using UnityEngine;

namespace Units
{
    [CreateAssetMenu(fileName = "UnitConfig", menuName = "Game/UnitConfig")]
    public class UnitConfig : ScriptableObject
    {
        [Header("Base")]
        public GameObject prefab;
        public GameObject ghostPrefab;
        
        [Header("Strategies")]
        public MovementType movementType;
        public AttackType attackType;

        [Header("VFX / Visuals")]
        public WeaponBase weaponData;
        
        [Header("Stats")]
        public int health;
        public float speed;
        public float aggressionRange = 5f; // Range at which the unit will start attacking enemies
    }
}