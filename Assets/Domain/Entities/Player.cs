namespace DefinitelyNotClashRoyale.Domain.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //
        public Deck Deck { get; set; }
        public Player(int id, string name)
        {
            Id = id;
            Name = name;
            Deck = new Deck();
        }
    }
}
