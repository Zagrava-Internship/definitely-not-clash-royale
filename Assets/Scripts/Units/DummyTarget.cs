using UnityEngine;

namespace Units
{
    public class DummyTarget : MonoBehaviour, ITargetable
    {
        [SerializeField] private float hp = 20;

        public Vector3 Position => transform.position;
        public bool IsDead => hp <= 0;

        public void TakeDamage(float amount)
        {
            hp -= amount;
            if (hp <= 0)
                Destroy(gameObject);
        }
    }

}