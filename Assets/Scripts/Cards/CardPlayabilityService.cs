using System;
using System.Collections.Generic;
using System.Linq;
using Mana;
using UnityEngine;

namespace Cards
{
    public class CardPlayabilityService:  MonoBehaviour
    {
        
        [Header("Provider")]
        [Tooltip("Assign a MonoBehaviour that implements IManaReadOnly (usually a ManaReadOnlyFacade).")]
        [SerializeField] private MonoBehaviour manaProvider;
        
        private IManaReadOnly _mana;
        
        private readonly Dictionary<CardData, bool> _registry = new();
        public event Action<CardData, bool> OnCardPlayabilityChanged;
        
        private void Awake()
        {
            if (manaProvider == null)
                throw new InvalidOperationException(
                    $"[{nameof(ManaUI)}] 'manaProvider' reference not assigned in inspector."
                );
            _mana = manaProvider as IManaReadOnly;
            if (_mana == null)
                throw new InvalidOperationException(
                    $"[{nameof(ManaUI)}] Assigned manaProvider does not implement IManaReadOnly."
                );
        }

        private void Start()
        {
            _mana.OnManaChanged += HandleManaChanged;
            HandleManaChanged(_mana.CurrentMana);
        }

        private void HandleManaChanged(float currentMana)
        {
            foreach (var (card, wasPlay) in _registry.ToList())
            {
                var nowPlay = currentMana >= card.Cost;
                if (nowPlay == wasPlay) continue;
                _registry[card] = nowPlay;
                OnCardPlayabilityChanged?.Invoke(card, nowPlay);
            }
        }

        public void Register(CardData card)
        {
            if (_registry.ContainsKey(card)) return;
            var playable = _mana.CurrentMana >= card.Cost;
            _registry[card] = playable;
            OnCardPlayabilityChanged?.Invoke(card, playable);
        }

        public void Unregister(CardData card)
        {
            _registry.Remove(card);
        }
        
        private void OnDestroy()
        { 
            _mana.OnManaChanged -= HandleManaChanged;
        }
    }
}