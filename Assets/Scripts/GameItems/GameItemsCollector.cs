using System.Collections.Generic;
using UnityEngine;

namespace GameItems
{
	[CreateAssetMenu(fileName = "GameItemsCollector", menuName = "GameItems/Collector")]
	public class GameItemsCollector : ScriptableObject
	{
		private static GameItemsCollector _instance;
		public static GameItemsCollector Instance
		{
			get
			{
				if (_instance == null)
					_instance = Resources.Load<GameItemsCollector>("Data/GameItemsCollector");

				return _instance;
			}
		}

		[SerializeField] List<GameItem> _items;

		public GameItem GetItem(int index)
		{
			if (index < 0 || index >= _items.Count)
				throw new System.Exception("There is no item with index like this");

			return _items[index];
		}
	}
}
