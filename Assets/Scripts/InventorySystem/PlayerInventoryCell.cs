using UnityEngine;
using UnityEngine.EventSystems;

namespace InventorySystem
{
	internal class PlayerInventoryCell : InventoryCell, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        private bool _isDragging = false;

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
