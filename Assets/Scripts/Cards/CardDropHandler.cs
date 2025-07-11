using Mana;
using Maps.MapManagement.Grid;
using Spawners;
using UnityEngine;

namespace Cards
{
    public class CardDropHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera mainCam;              // screen → world conversion
        [SerializeField] private float spawnZ;               // Z-depth for spawn plane
        
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

        }
        private void OnEnable()
        {
            CardDragHandler.OnCardDropped += HandleDrop;
        }

        private void OnDisable()
        {
            CardDragHandler.OnCardDropped -= HandleDrop;
        }

        private void HandleDrop(CardData card, Vector2 screenPos)
        {
            // 1) Convert screen to raw world position at desired Z
            var camZ = Mathf.Abs(mainCam.transform.position.z - spawnZ);
            var rawWorldPos = mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, camZ));
            rawWorldPos.z = spawnZ;
            
            // 2) Find closest grid node to that position
            var node = GridManager.Instance.GetPlaceableNodeFromWorldPoint(rawWorldPos);
            if (node is null)
            {
                Debug.LogWarning($"No valid grid node found for position {rawWorldPos}. Card drop failed.");
                return;
            }
            // 3) Snap spawn position to node.worldPosition if node exists
            var finalPos = node?.WorldPosition ?? rawWorldPos;
            finalPos.z = spawnZ; // Ensure Z is set correctly for spawning
            
            // 4) Spawn the unit at the final position
            UnitSpawner.Spawn(card.UnitToSpawn, finalPos);
            _manaSpender.Spend(card.Cost);
        }
    }

}