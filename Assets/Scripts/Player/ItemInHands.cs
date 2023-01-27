using UnityEngine;
using GameItems;
using InventorySystem;

namespace Player
{
	internal class ItemInHands : MonoBehaviour
	{
		internal IGameItemData ItemData { get; private set; }

		internal GameItemObject ItemObject { get; private set; }

		internal bool IsActive { get; private set; }

		internal void SetItem(IGameItemData item)
        {
			ItemData = null;
			if (ItemObject != null)
				Destroy(ItemObject.gameObject);

			if(item != null)
            {
				ItemData = item;

				GameItemObject newObjectPrefab = GameItemsCollector.Instance.GetItemObject(ItemData.Id);
				ItemObject = Instantiate(newObjectPrefab, transform);
				ItemObject.InitData(ItemData);
				ItemObject.gameObject.SetActive(IsActive);
			}
        }

		internal void Show()
		{
			if (ItemObject != null)
            {
				ItemObject.Enter();
			}

			IsActive = true;
		}
		internal void Hide(System.Action callBack)
		{
			if (ItemObject != null)
			{
				ItemObject.Exit(callBack);
			}
			IsActive = false;
		}
		internal void Interact()
        {
			if (IsActive && ItemObject != null)
				ItemObject.InteractUpdate();
		}
    }
}
