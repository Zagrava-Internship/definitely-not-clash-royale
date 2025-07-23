using System.Collections.Generic;
using System.Linq;
using Cards;
using UnityEngine;

namespace Deck
{
    public class DeckController: MonoBehaviour
    {
        [SerializeField] private CardData[] playerDeck;

        [SerializeField] private CardView[] handSlots;
        
        private Deck _deck;
        
        public IList<CardData> CurrentHand 
            => handSlots.Select(slot => slot.CardData)
                .Where(cd => cd != null)
                .ToList();
        private void Awake()
        {
            _deck = new Deck(playerDeck, handSlots.Length, new System.Random());
            Init();
        }
        
        private void Init()
        {
            for (var i = 0; i < handSlots.Length; ++i)
            {
                handSlots[i].Init(_deck.Hand[i], i); 
            }
            _deck.CardReplaced += OnCardReplaced;
        }

        private void OnCardReplaced(int index, CardData newCard)
        {
            handSlots[index].SetCard(newCard);
        }
        
        public void PlayCard(int index)
        {
            _deck.Play(index); 
        }
    }
}