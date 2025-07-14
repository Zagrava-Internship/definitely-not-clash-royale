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
        private bool _subscribed;

        private void Awake()
        {
            _view = GetComponent<CardView>();
            _mana = manaProvider as IManaReadOnly 
                    ?? throw new InvalidOperationException("manaProvider does not implement IManaReadOnly");
            _view.CardChanged += OnCardChanged;
        }

        private void Start()
        {
            TrySubscribe();
        }
        private void OnEnable()
        {
            TrySubscribe();
        }

        private void OnDisable()
        {
            if (!_subscribed) return;
            playabilityService.OnCardPlayabilityChanged -= HandlePlayabilityChanged;
            playabilityService.Unregister(_view.CardData);
            _subscribed = false;
        }
        
        
        private void TrySubscribe()
        {
            if (_subscribed) return;
            if (playabilityService == null || _mana == null || _view.CardData == null) return;

            playabilityService.OnCardPlayabilityChanged += HandlePlayabilityChanged;
            playabilityService.Register(_view.CardData);
            _subscribed = true;
        }
        
        private void OnCardChanged(CardData oldCard, CardData newCard)
        {
            playabilityService.Register(_view.CardData);
        }
        
        private void HandlePlayabilityChanged(CardData card, bool canPlay)
        {
            if (_view.CardData != card) return;
            _view.SetPlayableVisual(canPlay);
        }
    }
}