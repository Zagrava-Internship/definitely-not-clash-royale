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

            if (DropPositionUtils.IsValidDropScreenPosition(eventData.position, mainCam, ghostZ))
            {
                OnCardDropped?.Invoke(_cardView.SlotIndex, _cardView.CardData, eventData.position);
            }

            _ghost.Destroy();
        }
        
        private void CreateGhost(Vector2 screenPos)
        {
            var unitData = _cardView.CardData.UnitToSpawn;
            if (unitData == null || unitData.ghostPrefab == null) return;

            var snapPos = DropPositionUtils.ScreenToSnappedWorld(screenPos, mainCam, ghostZ);
            _ghost.Create(unitData, snapPos);

        }

        private void UpdateGhostPosition(Vector2 screenPos)
        {
            var snapPos = DropPositionUtils.ScreenToSnappedWorld(screenPos, mainCam, ghostZ);
            _ghost.Move(snapPos);
        }
        
    }
}
