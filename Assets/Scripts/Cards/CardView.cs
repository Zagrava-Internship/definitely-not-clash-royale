using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace Cards
{
    [RequireComponent(typeof(HID.Button))]
    public class CardView : MonoBehaviour
    {
        [SerializeField] private CardData cardData;
        [SerializeField] private Image backgroundImage; // button background
        [SerializeField] private Image iconImage;       // card icon

        public CardData CardData => cardData;
        public Image BackgroundImage => backgroundImage;

        private void Reset()
        {
            if (backgroundImage == null) 
                backgroundImage = GetComponent<Image>();
            if (iconImage == null) 
                iconImage = transform.Find("Image")?.GetComponent<Image>();
        }

        private void Start()
        {
            if (cardData == null)
            {
                Debug.LogError("CardView: cardData is not set");
                enabled = false;
                return;
            }

            backgroundImage.color = Color.white;
            if (cardData.Icon != null)
                iconImage.sprite = cardData.Icon;
            else
                iconImage.enabled = false;

        }
    }
}