using System;
using System.Collections.Generic;
using System.Linq;
using Cards;

namespace Deck
{
    public class Deck
    {
        private readonly Queue<CardData> _queue;
        private readonly List<CardData> _hand;
        public IReadOnlyList<CardData> Hand => _hand;

        public event Action<int, CardData> CardReplaced;

        public Deck(IEnumerable<CardData> allCards, int handSize, Random rng)
        {
            if (allCards == null) throw new ArgumentNullException(nameof(allCards));
            var cards = allCards.ToList();
            if (cards.Count < handSize)
                throw new ArgumentException("Not enough cards to fill hand.");
            if (handSize <= 0) throw new ArgumentOutOfRangeException(nameof(handSize));

            for (var i = cards.Count - 1; i > 0; --i)
            {
                var j = rng.Next(i + 1);
                (cards[i], cards[j]) = (cards[j], cards[i]);
            }

            _hand = new List<CardData>();
            for (var i = 0; i < handSize; ++i)
                _hand.Add(cards[i]);

            _queue = new Queue<CardData>(cards.Skip(handSize));
        }

        public void Play(int index)
        {
            if (index < 0 || index >= _hand.Count) throw new ArgumentOutOfRangeException(nameof(index));
            var played = _hand[index];

            _queue.Enqueue(played);

            var nextCard = _queue.Dequeue();
            _hand[index] = nextCard;

            CardReplaced?.Invoke(index, nextCard);
        }
    }
}