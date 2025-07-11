using UnityEngine;

namespace Units
{
    public class DummyTarget : TargetableBase
    {
        [SerializeField] private float hp = 20;

        public override Vector3 Position => transform.position;
        public override bool IsDead => hp <= 0;

        public override void TakeDamage(float amount)
        {
            hp -= amount;
            if (hp <= 0)
                Destroy(gameObject);
        }
    }

}