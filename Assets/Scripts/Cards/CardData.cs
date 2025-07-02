using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardData", menuName = "Game/CardData")]
    public class CardData : ScriptableObject
    {
        public string cardName;
        public Sprite icon;
        public UnitData unitToSpawn;
        public int cost;
    }
}

