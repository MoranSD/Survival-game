using UnityEngine;
using GameItems;
using UnityEngine.UI;
using TMPro;

namespace InventorySystem
{
    internal abstract class InventoryCell : MonoBehaviour
    {
        protected internal Inventory Inventory { get; set; }
        internal Vector2Int GridPosition { get; set; }
        internal bool IsEmpty { get; private set; } = true;

        [SerializeField] protected TextMeshProUGUI countTitle;
        [SerializeField] protected Image icon;


        internal void Render(BaseGameItemData itemData)
        {
            if(itemData.currentCount == 1) countTitle.text = "";
            else countTitle.text = itemData.currentCount.ToString();

            icon.gameObject.SetActive(true);
            icon.sprite = itemData.icon;

            IsEmpty = false;
        }
        internal void ResetRender()
        {
            countTitle.text = "";
            icon.gameObject.SetActive(false);
            IsEmpty = true;
        }

    }
}
