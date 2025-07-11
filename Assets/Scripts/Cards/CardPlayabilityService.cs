using System;
using System.Collections.Generic;
using Mana;
using UnityEngine;

namespace Cards
{
    public class CardPlayabilityService:  MonoBehaviour
    {
        [SerializeField] private ManaManager manaManager;
        private readonly Dictionary<CardData, bool> _registry = new();

        
        public event Action<CardData, bool> OnCardPlayabilityChanged;

        
        private void Awake()
        {
            if (manaManager == null)
                manaManager = ManaManager.Instance;
            manaManager.OnManaChanged += HandleManaChanged;
        }

        private void Start()
        {
            HandleManaChanged(manaManager.currentMana);
        }

        private void HandleManaChanged(float currentMana)
        {
            foreach (var (card, wasPlay) in new List<KeyValuePair<CardData,bool>>(_registry))
            {
                var nowPlay = currentMana >= card.cost;
                if (nowPlay == wasPlay) continue;
                _registry[card] = nowPlay;
                OnCardPlayabilityChanged?.Invoke(card, nowPlay);
            }
        }

        public void Register(CardData card)
        {
            if (_registry.ContainsKey(card)) return;
            var playable = manaManager.currentMana >= card.cost;
            _registry[card] = playable;
            OnCardPlayabilityChanged?.Invoke(card, playable);
        }

        public void Unregister(CardData card)
        {
            _registry.Remove(card);
        }
    }
}