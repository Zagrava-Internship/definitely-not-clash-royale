using Ghost;
using Maps.MapManagement.Grid;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards
{
    [RequireComponent(typeof(CardView), typeof(Image), typeof(CanvasGroup))]
    [DisallowMultipleComponent]
    public sealed class CardDragHandler : MonoBehaviour,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Tooltip("Assign a MonoBehaviour that implements ICardDragValidator (e.g., ManaCardDragValidator).")]
        [SerializeField] private MonoBehaviour manaValidatorProvider;
        [SerializeField] private CardPlayabilityService playabilityService;
        
        [Header("Visuals")]
        [SerializeField] private Camera mainCam;   // camera used to convert screen → world
        [SerializeField] private float ghostZ;    // Z-depth for the ghost prefab in world space
        
        private ICardDragValidator _validator;
        
        private CardView _cardView;  
        private CanvasGroup _canvasGroup;
        private int _slotIndex;
        
        private readonly GhostPreview _ghost = new();
        private bool _isDraggingAllowed;
        
        public static event System.Action<int, CardData, Vector2> OnCardDropped;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _cardView = GetComponent<CardView>();
            
            if (mainCam == null)
                throw new System.InvalidOperationException($"[{nameof(CardDragHandler)}] Main Camera not assigned in inspector.");

            if (manaValidatorProvider == null)
                throw new System.InvalidOperationException($"[{nameof(CardDragHandler)}] ManaValidatorProvider not assigned in inspector.");

            if (playabilityService == null)
                throw new System.InvalidOperationException($"[{nameof(CardDragHandler)}] PlayabilityService not assigned in inspector.");

            _validator = manaValidatorProvider as ICardDragValidator
                         ?? throw new System.InvalidOperationException(
                             $"[{nameof(CardDragHandler)}] Assigned validator provider does not implement ICardDragValidator");
            
            _cardView.CardChanged += OnCardChanged;
        }
        
        private void OnCardChanged(CardData oldCard, CardData newCard)
        {
            playabilityService.Register(newCard);
            UpdatePlayabilityVisual(newCard);
        }
        private void UpdatePlayabilityVisual(CardData card)
        {
            var canPlay = _validator.CanStartDrag(card);
            _cardView.SetPlayableVisual(canPlay);
        }
        
        private void Start()
        {
            playabilityService.OnCardPlayabilityChanged += OnPlayabilityChanged;
            playabilityService.Register(_cardView.CardData);
        }

        private void OnDestroy()
        {
            _cardView.CardChanged -= OnCardChanged;
            playabilityService.OnCardPlayabilityChanged -= OnPlayabilityChanged;
            playabilityService.Unregister(_cardView.CardData);
        }

        private void OnPlayabilityChanged(CardData card, bool canPlay)
        {
            if (card != _cardView.CardData) return;
            _cardView.BackgroundImage.color = canPlay ? Color.white : Color.gray;
            _canvasGroup.alpha = canPlay ? 1f : 0.6f;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_validator.CanStartDrag(_cardView.CardData))
            {
                Debug.Log($"Not enough mana to drag {_cardView.CardData.CardName}");
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
                OnCardDropped?.Invoke(_cardView.SlotIndex, _cardView.CardData, eventData.position);
            } 
            
            _ghost.Destroy();
        }
        
        private void CreateGhost(Vector2 screenPos)
        {
            var unitData = _cardView.CardData.UnitToSpawn;
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
