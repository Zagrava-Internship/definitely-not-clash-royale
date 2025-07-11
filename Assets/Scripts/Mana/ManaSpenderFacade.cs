using UnityEngine;

namespace Mana
{
    [RequireComponent(typeof(ManaManager))]
    public sealed class ManaSpenderFacade : MonoBehaviour, IManaSpender
    {
        private ManaManager _manaManager;

        private void Awake() => _manaManager = GetComponent<ManaManager>();

        public void Spend(int amount) => _manaManager.Spend(amount);
    }
}