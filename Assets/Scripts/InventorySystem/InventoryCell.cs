using UnityEngine;
using UnityEngine.EventSystems;
using GameItems;
using UnityEngine.UI;
using TMPro;

namespace InventorySystem
{
    public class InventoryCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        /*
		 * это будет инструмент для отображения предметов в инвентаре, и их менеджемнтом по нему
		 */
        public Vector2Int GridPosition { get; set; }
        public bool IsEmpty { get; private set; } = true;

        [SerializeField] private TextMeshProUGUI _countTitle;
        [SerializeField] private Image _icon;
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
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrop(PointerEventData eventData)
        {
            
        }
    }
}
