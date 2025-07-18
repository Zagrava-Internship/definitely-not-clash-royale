using Targeting;
using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "MeleeWeaponData", menuName = "Combat/MeleeWeaponData", order = 1)]
    public class MeleeWeaponData:WeaponBase
    {
        // Some melee weapons might have special properties or effects
        public override void Attack(ITargetable target)
        {
            if (target == null || target.IsDead)
            {
                Debug.LogWarning("Cannot attack a null or dead target.");
                return;
            }
            target.TakeDamage(Damage);
        }
    }
}