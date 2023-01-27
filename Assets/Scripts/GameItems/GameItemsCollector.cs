using System.Collections.Generic;
using UnityEngine;

namespace GameItems
{
	[CreateAssetMenu(fileName = "GameItemsCollector", menuName = "GameItems/Collector")]
	internal class GameItemsCollector : ScriptableObject
	{
		private static GameItemsCollector _instance;
		internal static GameItemsCollector Instance
		{
			get
			{
				if (_instance == null)
					_instance = Resources.Load<GameItemsCollector>("Data/GameItemsCollector");

				return _instance;
			}
		}

		[field: SerializeField] internal List<ItemInCollector> GameItems { get; private set; }

		internal IGameItemData GetItem(int index)
		{
			if (index < 0 || index >= GameItems.Count)
				throw new System.Exception("There is no item with index like this");

			return GameItems[index].ItemData;
		}
		internal GameItemObject GetItemObject(int index)
		{
			if (index < 0 || index >= GameItems.Count)
				throw new System.Exception("There is no item with index like this");

			return GameItems[index].ItemObject;
		}
	}
	[System.Serializable]
	internal struct ItemInCollector
    {
		[SerializeField] internal IGameItemData ItemData;
		[SerializeField] internal GameItemObject ItemObject;
    }
}
