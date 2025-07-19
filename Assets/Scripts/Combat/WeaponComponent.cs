using System;
using Targeting;
using UnityEngine;

namespace Combat
{
    public class WeaponComponent : MonoBehaviour
    {
        public WeaponBase weaponObject;

        public void OnEnable()
        {
            if (weaponObject != null) return;
            Debug.LogError("WeaponObject is not assigned in WeaponComponent on " + gameObject.name);
            return;
        }
    }
}