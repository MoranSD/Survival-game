using UnityEngine;

namespace GameItems
{
	internal interface IGameItemData
	{
		int Id { get; }
		Sprite Icon { get; }
		int MaxStackCount { get; }
		int CurrentCount { get; set; }
		bool IsStackable => MaxStackCount > 1;
	}
}
