using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace Cards
{
    public class CardButtonUi : MonoBehaviour
    {
        public CardData cardData;
        public GameManager gameManager;
        public Image iconImage;
        void Start()
        {
            iconImage.sprite = cardData.icon;
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        void OnClick()
        {
            gameManager.OnCardPlayed(cardData);
        }

    }
}
