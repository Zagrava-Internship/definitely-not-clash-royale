using System;
using Deck;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Image iconImage; // card icon
        
        public CardData CardData { get; private set; }
        public Image BackgroundImage { get; private set; }

        public int SlotIndex { get; private set; }

        public event Action<CardData /*old*/, CardData /*new*/> CardChanged;
        
        private  void Awake()
        {
            BackgroundImage = GetComponent<Image>();
        }
        public void Init(CardData initCardData, int index)
        {
            if (iconImage == null)
                throw new System.InvalidOperationException($"[{nameof(CardView)}] iconImage is not assigned in inspector.");
            if (initCardData == null)
                throw new System.ArgumentNullException(nameof(initCardData), "CardData must not be null.");
            if (index < 0)
                throw new System.ArgumentOutOfRangeException(nameof(index), "Index must be non-negative.");

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