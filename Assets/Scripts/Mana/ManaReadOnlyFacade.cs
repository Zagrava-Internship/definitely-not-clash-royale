using UnityEngine;

namespace Mana
{
    [RequireComponent(typeof(ManaManager))]
    public sealed class ManaReadOnlyFacade : MonoBehaviour, IManaReadOnly
    {
        private ManaManager _manaManager;
        private void Awake() => _manaManager = GetComponent<ManaManager>();

        public float CurrentMana => _manaManager.currentMana;
        public int MaxMana  => _manaManager.maxMana;

        public bool CanSpend(int amount) => _manaManager.CanSpend(amount);

        public event System.Action<float> OnManaChanged
        {
            add => _manaManager.OnManaChanged += value;
            remove => _manaManager.OnManaChanged -= value;
        }
    }
}