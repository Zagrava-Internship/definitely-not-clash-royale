using Ghost;
using Maps.MapManagement.Grid;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards
{
    [RequireComponent(typeof(Image), typeof(CanvasGroup))]
    [DisallowMultipleComponent]
    public sealed class CardDragHandler : MonoBehaviour,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        
        [Header("References")]
        [SerializeField] private CardView cardView;  
        [SerializeField] private MonoBehaviour manaValidatorProvider;
        [SerializeField] private CardPlayabilityService playabilityService;
        
        [Header("Visuals")]
        [SerializeField] private Camera mainCam;   // camera used to convert screen → world
        [SerializeField] private float ghostZ;    // Z-depth for the ghost prefab in world space


        private ICardDragValidator _validator;
        private CanvasGroup _canvasGroup;
        private readonly GhostPreview _ghost = new GhostPreview();
        private bool _isDraggingAllowed;
        
        public static event System.Action<CardData, Vector2> OnCardDropped;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            
            _validator = manaValidatorProvider as ICardDragValidator
                         ?? throw new System.InvalidOperationException(
                             $"{nameof(CardDragHandler)}: {nameof(manaValidatorProvider)} does not implement ICardDragValidator");

        }

        private void OnEnable()
        {
            playabilityService.OnCardPlayabilityChanged += OnPlayabilityChanged;
            playabilityService.Register(cardView.CardData);
        }

        private void OnDisable()
        {
            playabilityService.OnCardPlayabilityChanged -= OnPlayabilityChanged;
            playabilityService.Unregister(cardView.CardData);
        }

        private void OnPlayabilityChanged(CardData card, bool canPlay)
        {
            if (card != cardView.CardData) return;
            cardView.BackgroundImage.color = canPlay ? Color.white : Color.gray;
            _canvasGroup.alpha = canPlay ? 1f : 0.6f;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            ValidateData();
            
            if (!_validator.CanStartDrag(cardView.CardData))
            {
                Debug.Log($"Not enough mana to drag {cardView.CardData.cardName}");
                _isDraggingAllowed = false;
                return;
            }

            _isDraggingAllowed = true;
            _canvasGroup.blocksRaycasts = false;
            CreateGhost(eventData.position);
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDraggingAllowed) return;
            UpdateGhostPosition(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDraggingAllowed) return;
            _isDraggingAllowed = false;
            _canvasGroup.blocksRaycasts = true;
            
            var rawPos = ScreenToWorldWithZ(eventData.position, mainCam, ghostZ);
            var isValid = DropValidator.IsValidDropPosition(rawPos);

            if (isValid)
            {
                OnCardDropped?.Invoke(cardView.CardData, eventData.position);
            }
            
            _ghost.Destroy();
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
            var node = GridManager.Instance.GetNodeFromWorldPoint(rawPos);
            
            // 3) Snap spawn position to node.worldPosition if node exists
            var snapPos = node?.WorldPosition ?? rawPos;
            
            _ghost.Create(unitData, snapPos);
        }

        private void UpdateGhostPosition(Vector2 screenPos)
        {
            // same logic as CreateGhost, but move existing ghost instead of instantiating
            var rawPos = ScreenToWorldWithZ(screenPos, mainCam, ghostZ);
            var node = GridManager.Instance.GetNodeFromWorldPoint(rawPos);
            var snapPos = node?.WorldPosition ?? rawPos;
            _ghost.Move(snapPos);
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
