using Maps.MapManagement.Grid;
using Spawners;
using UnityEngine;

namespace Cards
{
    public class CardDropHandler : MonoBehaviour
    {
        public Camera mainCam;              // screen → world conversion
        public float spawnZ;               // Z-depth for spawn plane
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
            if (mainCam == null) {
                throw new System.InvalidOperationException(
                    $"{nameof(CardDropHandler)}: 'mainCam' reference is not assigned. Please set it in the inspector. (GameObject: {gameObject.name})"
                );
            }
            
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
            
            
            // 4) Spawn the unit at the final position
            UnitSpawner.Spawn(card.unitToSpawn, finalPos);
            Mana.ManaManager.Instance.Spend(card.cost);
        }
    }

}