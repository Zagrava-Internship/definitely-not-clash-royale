using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    [RequireComponent(typeof(Image), typeof(CanvasGroup), typeof(Button))]
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Image iconImage; // card icon
        
        public CardData CardData { get; private set; }
        public Image BackgroundImage { get; private set; }

        public int SlotIndex { get; private set; }

        public event Action<CardData /*old*/, CardData /*new*/> CardChanged;
        
        private CanvasGroup _canvasGroup;

        private  void Awake()
        {
            BackgroundImage = GetComponent<Image>();
            _canvasGroup = GetComponent<CanvasGroup>();
            if (BackgroundImage == null)
                throw new InvalidOperationException($"[{nameof(CardView)}] Image is not assigned or missing.");
            if (_canvasGroup == null)
                throw new InvalidOperationException($"[{nameof(CardView)}] CanvasGroup is not assigned or missing.");

        }
        public void Init(CardData initCardData, int index)
        {
            if (iconImage == null)
                throw new InvalidOperationException($"[{nameof(CardView)}] iconImage is not assigned in inspector.");
            if (initCardData == null)
                throw new ArgumentNullException(nameof(initCardData), "CardData must not be null.");
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be non-negative.");

            CardData = initCardData;
            SlotIndex = index;
            UpdateVisual();
        }

        public void SetCard(CardData data)
        {
            var old = CardData;
            CardData = data;
            UpdateVisual();
            CardChanged?.Invoke(old, data);
        }
        
        private void UpdateVisual()
        {
            BackgroundImage.sprite = CardData.Background;
            iconImage.enabled = true;
            iconImage.sprite = CardData.Icon;
        }

        public void SetPlayableVisual(bool canPlay)
        {
            BackgroundImage.color = canPlay ? Color.white : Color.gray;
            _canvasGroup.alpha = canPlay ? 1f : 0.6f; // transparency for non-playable cards
        }
        
        private void Start()
        {
            if (CardData == null)
            {
                Debug.LogError("CardView: cardData is not set");
                enabled = false;
                return;
            }

            BackgroundImage.color = Color.white;
            if (CardData.Icon != null)
                iconImage.sprite = CardData.Icon;
            else
                iconImage.enabled = false;

        }

    }
}