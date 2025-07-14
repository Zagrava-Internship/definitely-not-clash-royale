using System;
using UnityEngine;

namespace Health
{
    public class HealthComponent: MonoBehaviour
    {
        public int Max { get; private set; }
        public int Current { get; private set; }

        public event Action OnHealthChanged;
        public event Action OnDied;

        public void Setup(int maxHP)
        {
            Max = maxHP;
            Current = maxHP;
        }

        public void TakeDamage(int amount)
        {
            Current -= amount;
            Current = Mathf.Clamp(Current, 0, Max);

            OnHealthChanged?.Invoke();

            if (Current <= 0)
            {
                OnDied?.Invoke();
            }
        }

        public void Heal(int amount)
        {
            Current += amount;
            Current = Mathf.Clamp(Current, 0, Max);

            OnHealthChanged?.Invoke();
        }
    }
}