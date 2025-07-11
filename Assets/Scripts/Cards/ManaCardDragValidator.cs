using System;
using Mana;
using UnityEngine;

namespace Cards
{
    public sealed class ManaCardDragValidator : MonoBehaviour, ICardDragValidator
    {
        [Header("Provider")]
        [Tooltip("Assign a MonoBehaviour that implements IManaReadOnly (usually a ManaReadOnlyFacade).")]
        [SerializeField] private MonoBehaviour manaProvider;

        private IManaReadOnly _mana;

        private void Awake()
        {
            _mana = manaProvider as IManaReadOnly;
            if (_mana != null) return;
            var providerName = manaProvider != null ? manaProvider.GetType().Name : "null";
            throw new InvalidOperationException(
                $"ManaCardDragValidator: Assigned manaProvider ({providerName}) does not implement IManaReadOnly. " +
                "Please assign a component like ManaReadOnlyFacade."
            );
        }

        public bool CanStartDrag(CardData card)
        {
            return _mana.CanSpend(card.Cost);
        }
    }

}