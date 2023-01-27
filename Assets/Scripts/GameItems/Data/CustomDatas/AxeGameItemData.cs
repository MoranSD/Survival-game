using UnityEngine;
using Enums;
using System.Collections.Generic;

namespace GameItems
{
	[CreateAssetMenu(fileName = "Axe Item Data", menuName = "GameItems/Data/AxeItem data")]
	internal class AxeGameItemData : ScriptableObject, IGameItemData
	{
        [field: Header("Статичные поля")]
        [field: SerializeField] internal int Id { get; set; }
        [field: SerializeField] internal Sprite Icon { get; set; }
        [field: SerializeField] internal int MaxStackCount { get; set; }
        [field: SerializeField] internal int MaxEndurance { get; set; }
        [field: SerializeField] internal int Damage { get; set; }
        [field: SerializeField] internal int DamageMultiplyForTypeTarget { get; set; }
        [field: SerializeField] internal List<DamageableTargetType> TargetTypes { get; set; }
        [field: Header("Динамические поля")]
        [field: SerializeField] internal int CurrentCount { get; set; }
        [field: SerializeField] internal int CurrentEndurance { get; set; }

        int IGameItemData.Id => Id;
        Sprite IGameItemData.Icon => Icon;
        int IGameItemData.MaxStackCount => MaxStackCount;
        int IGameItemData.CurrentCount { get => CurrentCount; set => CurrentCount = value; }
    }
}
