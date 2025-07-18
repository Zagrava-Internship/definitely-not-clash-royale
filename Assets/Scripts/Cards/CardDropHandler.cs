using Deck;
using Mana;
using Maps.MapManagement.Grid;
using Spawners;
using Targeting;
using UnityEngine;

namespace Cards
{
    public class CardDropHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera mainCam;              // screen → world conversion
        [SerializeField] private float spawnZ;               // Z-depth for spawn plane
        [SerializeField] private DeckController deckController;

        [Header("Mana")]
        [Tooltip("Assign a MonoBehaviour that implements IManaSpender (usually ManaSpenderFacade).")]
        [SerializeField] private MonoBehaviour manaSpenderProvider;

        private IManaSpender _manaSpender;

        private void Awake()
        {
            if (mainCam == null)
                throw new System.InvalidOperationException(
                    "[{nameof(CardDropHandler)}] Main Camera not assigned in inspector."
                );
            if (manaSpenderProvider == null)
                throw new System.InvalidOperationException(
                    $"[{nameof(CardDropHandler)}] 'manaSpenderProvider' reference not assigned in inspector."
                );
            
            _manaSpender = manaSpenderProvider as IManaSpender;
            if (_manaSpender == null)
                throw new System.InvalidOperationException(
                    $"[{nameof(CardDropHandler)}] Assigned manaSpenderProvider does not implement IManaSpender."
                );

            if (deckController == null)
                throw new System.InvalidOperationException(
                    $"[{nameof(CardDropHandler)}] DeckController reference not assigned in inspector."
                );
        }
        private void OnEnable()
        {
            CardDragHandler.OnCardDropped += HandleDrop;
        }

        private void OnDisable()
        {
            CardDragHandler.OnCardDropped -= HandleDrop;
        }

        private void HandleDrop(int index, CardData card, Vector2 screenPos)
        {
            var finalPos = DropPositionUtils.ScreenToSnappedWorld(screenPos, mainCam, spawnZ);
            var node = GridManager.Instance.GetPlaceableNodeFromWorldPoint(finalPos);
            if (node == null)
            {
                Debug.LogWarning($"No valid grid node found for position {finalPos}. Card drop failed.");
                return;
            }
            
            UnitSpawner.Spawn(card.UnitToSpawn, finalPos, TeamIds.Team1);
            _manaSpender.Spend(card.Cost);
            
            deckController.PlayCard(index);
        }
    }

}