using DefinitelyNotClashRoyale.Domain.Enums;

namespace DefinitelyNotClashRoyale.Domain.Entities
{
    public class Card
    {
        public Card(int id, string name, int elixirCost, CardType cardType)
        {
            Id = id;
            Name = name;
            ElixirCost = elixirCost;
            CardType = cardType;
        }

        public int Id { get; }
        public string Name { get; }
        public int ElixirCost { get; }
        public CardType CardType { get; }
    }
}