namespace Cards
{
    public interface ICardDragValidator
    {
        public bool CanStartDrag(CardData card);
    }
}