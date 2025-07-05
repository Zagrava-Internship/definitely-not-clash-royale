using Grid;
using UnityEngine;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        private UnitData _data;
        private int _currentHealth;
        private Vector2Int _followingPosition;

        public void Initialize(UnitData unitData,Vector2Int position)
        {
            if (!unitData)
            {
                Debug.LogError("UnitData is null. Please assign a valid UnitData.");
                return;
            }
            var grid = GridManager.Instance;
            _data = unitData;
            _currentHealth = _data.health;
            _followingPosition = position;
            GetComponent<GridMover>().MoveTo(GridManager.Instance.GetNode(_followingPosition));
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
