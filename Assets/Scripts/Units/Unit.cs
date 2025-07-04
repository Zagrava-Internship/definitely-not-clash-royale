using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        private UnitData _data;
        private int _currentHealth;

        public void Initialize(UnitData unitData)
        {
            _data = unitData;
            _currentHealth = _data.health;
        }

        public void TakeDamage(int amount)
        {
            _currentHealth -= amount;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
