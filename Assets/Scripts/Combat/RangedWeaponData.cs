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

        public override void Attack(ITargetable target)
        {
            if (target == null || target.IsDead)
            {
                Debug.LogWarning("Cannot attack a null or dead target.");
                return;
            }

            // Check if the target is flying and if this weapon can target flying units
            // TODO : Make some stuff to check if the target is flying

            ParticleManager.SpawnParticle(particlePrefab, target.Transform,
                () => {
                    // This is called when the particle has followed the target
                    target.TakeDamage(Damage);
                });
        }
    }
}