using UnityEngine;
using GameItems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    internal class InventoryCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        protected internal Inventory Inventory { get; set; }
        internal Vector2Int GridPosition { get; set; }
        internal bool IsEmpty { get; private set; } = true;

        [SerializeField] protected TextMeshProUGUI countTitle;
        [SerializeField] protected Image icon;

        private bool _isDragging = false;

        internal void Render(IGameItemData itemData)
        {
            if(itemData.CurrentCount == 1) countTitle.text = "";
            else countTitle.text = itemData.CurrentCount.ToString();

            icon.gameObject.SetActive(true);
            icon.sprite = itemData.Icon;

            IsEmpty = false;
        }
        internal void ResetRender()
        {
            countTitle.text = "";
            icon.gameObject.SetActive(false);
            IsEmpty = true;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Inventory.Items.ContainsKey(GridPosition))
                _isDragging = true;

            icon.raycastTarget = false;
            icon.transform.SetParent(Inventory.UIInventory.MainCanvas.transform);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDragging == false) return;

            icon.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_isDragging == false) return;

            icon.raycastTarget = true;
            icon.transform.SetParent(transform);
            icon.transform.localPosition = Vector3.zero;
            _isDragging = false;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.transform.parent != null)
            {
                InventoryCell dragCell = eventData.pointerDrag.GetComponentInParent<InventoryCell>();

                if (dragCell != null)
                    Inventory.TryMergeCells(dragCell, this);
            }
        }
    }
}
