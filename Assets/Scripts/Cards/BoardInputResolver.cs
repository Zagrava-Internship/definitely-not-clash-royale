using UnityEngine;

namespace Cards
{
    public class BoardInputResolver : MonoBehaviour
    {
        public Camera mainCam;             
        public UnitSpawner spawner;
        public float spawnZ;
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
            if (mainCam == null || spawner == null) {
                Debug.LogError("BoardInputResolver: 'mainCam' or 'spawner' reference is missing. Please assign them in the inspector.");
                return;
            }
            var camZ = Mathf.Abs(mainCam.transform.position.z - spawnZ);
            var worldPos = mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, camZ));
            worldPos.z = spawnZ; 
            UnitSpawner.Spawn(card.unitToSpawn, worldPos);
        }
    }

}