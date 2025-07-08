using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards
{
    [RequireComponent(typeof(Image))]
    [DisallowMultipleComponent]
    public sealed class CardDragHandler : MonoBehaviour,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        
        [Header("References")]
        [SerializeField] private CardView cardView;  
        
        [Header("Visuals")]
        [SerializeField] private Camera mainCam;   // camera used to convert screen → world
        
        [SerializeField] private float ghostZ;    // Z-depth for the ghost prefab in world space

        private CanvasGroup _canvasGroup;
        private GameObject _ghostInstance;

        private bool _isDraggingAllowed;
        public static event System.Action<CardData, Vector2> OnCardDropped;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

        }

        private void OnEnable()
        {
            Mana.ManaManager.OnManaChanged += OnManaChanged;
            UpdateVisualState();
        }

        private void OnDisable()
        {
            Mana.ManaManager.OnManaChanged -= OnManaChanged;
        }

        private void OnManaChanged(float mana) => UpdateVisualState();

        private void UpdateVisualState()
        {
            if (Mana.ManaManager.Instance.CanSpend(cardView.CardData.cost))
            {
                cardView.BackgroundImage.color = Color.white;
                _canvasGroup.alpha = 1f;
            }
            else
            {
                cardView.BackgroundImage.color = Color.gray;
                _canvasGroup.alpha = 0.6f;
            }
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            ValidateData();

            if (!Mana.ManaManager.Instance.CanSpend(cardView.CardData.cost))
            {
                Debug.Log($"Not enough mana to drag {cardView.CardData.cardName}");
                _isDraggingAllowed = false;
                return;
            }

            _isDraggingAllowed = true;
            
            _canvasGroup.blocksRaycasts = false; // allow raycasts to pass through while dragging
            CreateGhost(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDraggingAllowed) return;
            
            //  move ghost with snap logic
            if (_ghostInstance != null)
                UpdateGhostPosition(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDraggingAllowed) return;
            _isDraggingAllowed = false;
            
            _canvasGroup.blocksRaycasts = true;
            OnCardDropped?.Invoke(cardView.CardData, eventData.position);
            
            // clean up ghost
            if (_ghostInstance != null) Destroy(_ghostInstance);
        }
    
        private void ValidateData()
        {
            if (cardView  == null)
                throw new System.InvalidOperationException($"{nameof(CardDragHandler)}: CardView not assigned! (obj: {gameObject.name})");

            if (mainCam == null)
                throw new System.InvalidOperationException($"{nameof(CardDragHandler)}: Main Camera not assigned! (obj: {gameObject.name})");
        }



        private void CreateGhost(Vector2 screenPos)
        {
            var unitData = cardView.CardData.unitToSpawn;
            if (unitData == null || unitData.ghostPrefab == null) return;

            // 1) Convert screen to raw world position at desired Z
            var rawPos = ScreenToWorldWithZ(screenPos, mainCam, ghostZ);
            
            // 2) Find closest grid node to that position
            var node = Grid.GridManager.Instance.GetClosestNode(rawPos);
            
            // 3) Snap spawn position to node.worldPosition if node exists
            var snapPos = node?.worldPosition ?? rawPos;
            
            
            // Instantiate the ghost at snapped position
            _ghostInstance = Instantiate(unitData.ghostPrefab, snapPos, Quaternion.identity);
        }

        private void UpdateGhostPosition(Vector2 screenPos)
        {
            // same logic as CreateGhost, but move existing ghost instead of instantiating
            var rawPos = ScreenToWorldWithZ(screenPos, mainCam, ghostZ);
            var node = Grid.GridManager.Instance.GetClosestNode(rawPos);
            _ghostInstance.transform.position = node?.worldPosition ?? rawPos;
        }

        private static Vector3 ScreenToWorldWithZ(Vector2 screenPos, Camera cam, float ghostZ)
        {
            // project screen point into world at distance cam->Z, then set ghost Z explicitly
            var worldZ = Mathf.Abs(cam.transform.position.z);
            var pos = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, worldZ));
            pos.z = ghostZ;
            return pos;
        }
    }
}
