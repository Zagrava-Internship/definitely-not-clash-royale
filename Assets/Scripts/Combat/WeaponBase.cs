using Targeting;
using UnityEngine;

namespace Combat
{
    public abstract class WeaponBase:ScriptableObject,IWeapon
    {
        [field:Header("Weapon Properties")]
        [field: SerializeField]public int Damage { get; set; }
        [field: SerializeField]public float AttackRange { get; set; }
        [field: SerializeField]public float AttackSpeed { get; set; }
        [field: SerializeField]public float AttackDelay { get; set; }

        public abstract void Attack(ITargetable target);
        //[Header("Audio")]
        //public AudioClip attackSound;
        //public AudioClip hitSound;
    }
}