using System;
using Targeting;
using UnityEngine;

namespace Combat
{
    public class WeaponComponent:MonoBehaviour
    {
        public WeaponBase weaponObject;
        public void OnEnable()
        {
            if (weaponObject != null) return;
            Debug.LogError("WeaponObject is not assigned in WeaponComponent on " + gameObject.name);
            return;
        }
        public void Attack(ITargetable target)
        {
            if (weaponObject == null)
            {
                Debug.LogError("Weapon data is not initialized. Cannot perform attack.");
                return;
            }
            if (target == null)
            {
                Debug.LogError("Target is null. Cannot perform attack.");
                return;
            }
            // Perform the attack logic
            weaponObject.Attack(target);
        }
    }
}