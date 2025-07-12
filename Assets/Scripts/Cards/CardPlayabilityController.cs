using System;
using Mana;
using UnityEngine;

namespace Cards
{
    [RequireComponent(typeof(CardView))]
    public class CardPlayabilityController: MonoBehaviour
    {
        [SerializeField] private CardPlayabilityService playabilityService;
        [Tooltip("Assign a MonoBehaviour that implements IManaReadOnly (usually a ManaReadOnlyFacade).")]
        [SerializeField] private MonoBehaviour manaProvider;

        private CardView _view;
        private IManaReadOnly _mana;

        private void Awake()
        {
            _view = GetComponent<CardView>();
            _mana = manaProvider as IManaReadOnly 
                    ?? throw new InvalidOperationException("manaProvider does not implement IManaReadOnly");
            _view.CardChanged += OnCardChanged;
        }

        private void Start()
        {
            playabilityService.OnCardPlayabilityChanged += HandlePlayabilityChanged;
            Register(_view.CardData);
        }

        private void OnDestroy()
        {
            playabilityService.OnCardPlayabilityChanged -= HandlePlayabilityChanged;
            playabilityService.Unregister(_view.CardData);
        }
        
        private void OnCardChanged(CardData oldCard, CardData newCard)
        {
            Register(newCard);
        }
        
        private void Register(CardData card)
        {
            playabilityService.Register(card);
            var canPlay = _mana.CurrentMana >= card.Cost;
            _view.SetPlayableVisual(canPlay);
        }
        
        private void HandlePlayabilityChanged(CardData card, bool canPlay)
        {
            if (_view.CardData != card) return;
            _view.SetPlayableVisual(canPlay);
        }
    }
}