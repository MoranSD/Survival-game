using UnityEngine;
using UnityEngine.EventSystems;
using GameItems;
using UnityEngine.UI;
using TMPro;

namespace InventorySystem
{
    public class InventoryCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        public Vector2Int GridPosition { get; set; }
        public bool IsEmpty { get; private set; } = true;

        [SerializeField] private TextMeshProUGUI _countTitle;
        [SerializeField] private Image _icon;

        private bool _isDragging = false;

        public void Render(BaseGameItemData itemData)
        {
            if(itemData.currentCount == 1) _countTitle.text = "";
            else _countTitle.text = itemData.currentCount.ToString();

            _icon.gameObject.SetActive(true);
            _icon.sprite = itemData.icon;

            IsEmpty = false;
        }
        public void ResetRender()
        {
            _countTitle.text = "";
            _icon.gameObject.SetActive(false);
            IsEmpty = true;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Inventory.Instance.Items.ContainsKey(GridPosition))
                _isDragging = true;

            _icon.raycastTarget = false;
            _icon.transform.SetParent(Inventory.Instance.UIInventory.MainCanvas.transform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDragging == false) return;

            _icon.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isDragging == false) return;

            _icon.raycastTarget = true;
            _icon.transform.SetParent(transform);
            _icon.transform.localPosition = Vector3.zero;
            _isDragging = false;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if(eventData.pointerDrag.transform.parent != null)
            {
                InventoryCell dragCell = eventData.pointerDrag.GetComponentInParent<InventoryCell>();

                if (dragCell != null)
                    Inventory.Instance.TryMergeCells(dragCell, this);
            }
        }
    }
}
