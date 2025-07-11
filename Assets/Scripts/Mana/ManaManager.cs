using System;
using Unity.Collections;
using UnityEngine;

namespace Mana
{
    public class ManaManager : MonoBehaviour
    {
        [Header("Mana Settings")] 
        public int maxMana = 10;
        public float regenRate = 1f;      // mana per second
        [ReadOnly] public float currentMana;
        public float initialMana = 0f; // initial mana at start

        public event Action<float> OnManaChanged;

        private void Awake()
        {
            currentMana = initialMana;
        }

        private void Update()
        {
            if (!(currentMana < maxMana)) return;
            currentMana = Mathf.Min(maxMana, currentMana + regenRate * Time.deltaTime);
            OnManaChanged?.Invoke(currentMana);
        }

        public bool CanSpend(int amount) => currentMana >= amount;

        public void Spend(int amount)
        {
            currentMana = Mathf.Max(0, currentMana - amount);
            OnManaChanged?.Invoke(currentMana);
        }
    }
}