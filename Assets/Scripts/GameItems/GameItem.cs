using UnityEngine;

namespace GameItems
{
	public abstract class GameItem : MonoBehaviour
	{
		/*
		 * это будет сам игровой обьект
		 */
		[field: SerializeField] public BaseGameItemData BaseData { get; protected set; }
		public object UnicData { get; protected set; }
		//по нужному пути, будет искать нужный файл, и в зависимости от типа, выгружать как надо и применять к себе
		public abstract void LoadData(string DataPath);
    }
	[System.Serializable]
	public struct BaseGameItemData
    {
		public int Id;
		public Sprite icon;
		public int currentCount;
		public int maxStackCount;
		public bool IsStackable => maxStackCount > 1;
	}
}
