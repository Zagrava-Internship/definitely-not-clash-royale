using Combat.Particles;
using Targeting;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "RangedWeaponData", menuName = "Combat/RangedWeaponData", order = 2)]
    public class RangedWeaponData:WeaponBase
    {
        [Header("Targeting")]
        public bool canTargetFlying = false; // Can this weapon target flying units?
        [Header("Visuals")]
        public GameObject particlePrefab; // Prefab for the attack particle
    }
}