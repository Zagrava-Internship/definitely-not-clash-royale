using Units;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardData", menuName = "Game/CardData")]
    public class CardData : ScriptableObject
    {
        [Header("Card Info")]
        [SerializeField, Tooltip("Display name of the card.")]
        private string cardName;

        [SerializeField, Tooltip("Card icon.")]
        private Sprite icon;

        [Header("Spawn Data")]
        [SerializeField, Tooltip("Unit to spawn when this card is played.")]
        private UnitData unitToSpawn;
        
        [SerializeField, Tooltip("Mana cost to play the card.")]
        private int cost = 1;
        
        public string CardName => cardName;
        public Sprite Icon => icon;
        public UnitData UnitToSpawn => unitToSpawn;
        public int Cost => cost;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (cost >= 0) return;
            Debug.LogWarning($"[{nameof(CardData)}] Card '{cardName}' has negative cost: {cost}.");
            cost = 0;
        }
#endif

    }
}

