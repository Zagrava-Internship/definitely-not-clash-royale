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
        [Header("Data")]
        [SerializeField] private CardData cardData;       
        [Header("Visuals")]
        [SerializeField] private Camera mainCam;  
        
        [SerializeField] private float ghostZ;

        private Canvas _rootCanvas;
        private CanvasGroup _canvasGroup;
        private GameObject _previewInstance;
        private GameObject _ghostInstance;

        public static event System.Action<CardData, Vector2> OnCardDropped;

        private void Awake()
        {
            _rootCanvas  = GetComponentInParent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!ValidateData()) return;

            _canvasGroup.blocksRaycasts = false;
            CreatePreview(eventData.position);
            CreateGhost(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_previewInstance != null)
                _previewInstance.transform.position = eventData.position;

            if (_ghostInstance != null)
                UpdateGhostPosition(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            OnCardDropped?.Invoke(cardData, eventData.position);
            if (_previewInstance != null) Destroy(_previewInstance);
            if (_ghostInstance != null) Destroy(_ghostInstance);
        }
    
        private bool ValidateData()
        {
            if (cardData == null)
            {
                Debug.LogWarning($"{nameof(CardDragHandler)}: CardData not assigned!", this);
                return false;
            }
            if (mainCam == null)
            {
                Debug.LogWarning($"{nameof(CardDragHandler)}: Main Camera not assigned!", this);
                return false;
            }
            return true;
        }

        private void CreatePreview(Vector2 screenPos)
        {
            var img = new GameObject("CardPreview", typeof(RectTransform), typeof(Image)).GetComponent<Image>();
            img.sprite = GetComponent<Image>()?.sprite;
            img.raycastTarget = false;
            _previewInstance = img.gameObject;
            _previewInstance.transform.SetParent(_rootCanvas.transform, false);
            _previewInstance.transform.SetAsLastSibling();
            _previewInstance.transform.position = screenPos;
        }

        private void CreateGhost(Vector2 screenPos)
        {
            var unitData = cardData.unitToSpawn;
            if (unitData == null || unitData.ghostPrefab == null) return;

            var worldPos = ScreenToWorldWithZ(screenPos, mainCam, ghostZ);
            _ghostInstance = Instantiate(unitData.ghostPrefab, worldPos, Quaternion.identity);
        }

        private void UpdateGhostPosition(Vector2 screenPos)
        {
            var worldPos = ScreenToWorldWithZ(screenPos, mainCam, ghostZ);
            _ghostInstance.transform.position = worldPos;
        }

        private static Vector3 ScreenToWorldWithZ(Vector2 screenPos, Camera cam, float ghostZ)
        {
            var worldZ = Mathf.Abs(cam.transform.position.z);
            var pos = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, worldZ));
            pos.z = ghostZ;
            return pos;
        }
    }
}
