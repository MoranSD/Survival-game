using UnityEngine;

namespace GameItems
{
	public abstract class GameItem : MonoBehaviour
	{
		/*
		 * ��� ����� ��� ������� ������
		 */
		[field: SerializeField] public BaseGameItemData BaseData { get; protected set; }
		public object UnicData { get; protected set; }
		//�� ������� ����, ����� ������ ������ ����, � � ����������� �� ����, ��������� ��� ���� � ��������� � ����
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
