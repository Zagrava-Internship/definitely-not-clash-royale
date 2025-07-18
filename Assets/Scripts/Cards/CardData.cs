using Units;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardData", menuName = "Game/CardData")]
    public class CardData : ScriptableObject
    {
        
        [Header("Card Info")]
        [Tooltip("Display name of the card.")]
        [SerializeField] private string cardName;

        [Header("Visuals")]
        [Tooltip("Background image (Sprite, not Image).")]
        [SerializeField] private Sprite background;
        [Tooltip("Card icon.")]
        [SerializeField] private Sprite icon;
        
        [Header("Spawn Data")]
        [Tooltip("Unit to spawn when this card is played.")]
        [SerializeField] private UnitConfig unitToSpawn;
        [Tooltip("Mana cost to play the card.")]
        [SerializeField] private int cost = 1;
        
        public string CardName => cardName;
        public Sprite Icon => icon;
        public Sprite Background => background;
        public UnitConfig UnitToSpawn => unitToSpawn;
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

