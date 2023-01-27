using UnityEngine;
using GameItems;
using Cysharp.Threading.Tasks;

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
				ItemObject.transform.localPosition = Vector3.zero;
				ItemObject.InitData(ItemData);

				if (IsActive) ItemObject.Enter();
				else ItemObject.Exit();
			}
        }

		async internal void Show()
		{
			if (ItemObject != null)
            {
				ItemObject.Enter();
				await UniTask.WaitWhile(() => ItemObject.IsActive == false);
			}

			IsActive = true;
		}
		async internal void Hide()
		{
			if (ItemObject != null)
            {
				ItemObject.Exit();
				await UniTask.WaitWhile(() => ItemObject.IsActive);
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
