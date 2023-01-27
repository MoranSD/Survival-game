using System.Collections.Generic;
using UnityEngine;
using GameItems;

namespace InventorySystem
{
    internal class Inventory
    {
        public event System.Action<int> OnUpdateItemInFastSlots;

        internal Dictionary<Vector2Int, IGameItemData> Items { get; private set; }
        internal UIInventory UIInventory { get; private set; }

        internal int Columns { get; private set; }
        internal int Rows { get; private set; }
        internal int FastSlotsCount { get; private set; }

        internal Inventory(Canvas mainCanvas, GameObject menuPanel, Transform mainCellsContainer, Transform fastCellsContainer, InventoryCell cellPrefab, int columns = 5, int rows = 5, int fastSlotsCount = 5)
        {
            Columns = columns;
            Rows = rows;
            FastSlotsCount = fastSlotsCount;

            Items = new Dictionary<Vector2Int, IGameItemData>();
            UIInventory = new UIInventory(mainCanvas, menuPanel, mainCellsContainer, fastCellsContainer, cellPrefab);
            UIInventory.Hide();
            UIInventory.CreateCells(this);
        }
        internal Inventory(Canvas mainCanvas, GameObject menuPanel, Transform mainCellsContainer, InventoryCell cellPrefab, int columns = 5, int rows = 5)
        {
            Columns = columns;
            Rows = rows;
            FastSlotsCount = 0;

            Items = new Dictionary<Vector2Int, IGameItemData>();
            UIInventory = new UIInventory(mainCanvas, menuPanel, mainCellsContainer, cellPrefab);
            UIInventory.Hide();
            UIInventory.CreateCells(this);
        }
        internal bool TryMergeCells(InventoryCell dragCell, InventoryCell mergeCell)//mergeCell это всегда наша €чейка, dragCell может быть из другого инвентар€
        {
            if (IsHaveFreePositions() == false) return false;

            if(dragCell.Inventory.Items.ContainsKey(dragCell.GridPosition) == false)
                throw new System.Exception("You trying to drag empty cell");

            IGameItemData dragItemData = dragCell.Inventory.Items[dragCell.GridPosition];

            if (Items.ContainsKey(mergeCell.GridPosition))
            {
                IGameItemData mergeItemData = Items[mergeCell.GridPosition];

                if (dragItemData.Id == mergeItemData.Id && dragItemData.IsStackable)
                {
                    int freeSlotsCount = mergeItemData.MaxStackCount - mergeItemData.CurrentCount;
                    if (freeSlotsCount >= dragItemData.CurrentCount)
                    {
                        Items[mergeCell.GridPosition].CurrentCount += dragItemData.CurrentCount;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                Items.Add(mergeCell.GridPosition, dragItemData);
            }

            dragCell.Inventory.RemoveItem(dragCell.GridPosition);
            UIInventory.UpdateCell(mergeCell.GridPosition, Items[mergeCell.GridPosition]);
            return true;
        }
        internal bool IsHaveFreePositions()
        {
            int maxSlotsCount = (Columns * Rows) + FastSlotsCount;

            return Items.Count < maxSlotsCount;
        }
        internal bool TryAddItem(IGameItemData gameItem)
        {
            if (gameItem.IsStackable)
            {
                if(GetFreePositionWithStackableItem(out Vector2Int position, gameItem.Id, gameItem.CurrentCount))
                {
                    AddItemInPosition(position);
                    return true;
                }
            }
            else
            {
                if(GetFreePosition(out Vector2Int position))
                {
                    AddItemInPosition(position);
                    return true;
                }
            }

            void AddItemInPosition(Vector2Int position)
            {
                if(Items.ContainsKey(position)) Items[position].CurrentCount += gameItem.CurrentCount;
                else Items.Add(position, gameItem);
                UIInventory.UpdateCell(position, Items[position]);
                if (position.y == -1) OnUpdateItemInFastSlots?.Invoke(position.x);
            }

            return false;
        }
        internal void RemoveItem(Vector2Int position)
        {
            if (Items.ContainsKey(position))
            {
                Items.Remove(position);
                UIInventory.UpdateCell(position, null);
                if (position.y == -1) OnUpdateItemInFastSlots?.Invoke(position.x);
            }
        }
        internal bool TryGetItem(Vector2Int position, out IGameItemData item)
        {
            if (Items.ContainsKey(position))
            {
                item = Items[position];
                Items.Remove(position);
                UIInventory.UpdateCell(position, null);
                if (position.y == -1) OnUpdateItemInFastSlots?.Invoke(position.x);
                return true;
            }

            item = null;
            return false;
        }
        internal bool TryLoadData(Dictionary<Vector2Int, IGameItemData> data)
        {
            return false;
        }
        internal IGameItemData GetItemFromFastSlots(int slotId)
        {
            if (slotId < 0 || slotId >= FastSlotsCount)
                throw new System.Exception("There is no fast slot with this id");

            Vector2Int slotPosition = new Vector2Int(slotId, -1);

            if (Items.ContainsKey(slotPosition)) return Items[slotPosition];
            else return null;
        }
        private bool GetFreePosition(out Vector2Int position)
        {
            for (int y = Columns - 1; y >= 0; y--)
            {
                for (int x = 0; x < Rows; x++)
                {
                    Vector2Int nextCellPosition = new Vector2Int(x, y);
                    if (Items.ContainsKey(nextCellPosition) == false)
                    {
                        position = nextCellPosition;
                        return true;
                    }
                }
            }

            for (int x = 0; x < FastSlotsCount; x++)
            {
                Vector2Int nextCellPosition = new Vector2Int(x, -1);
                if (Items.ContainsKey(nextCellPosition) == false)
                {
                    position = nextCellPosition;
                    return true;
                }
            }

            position = Vector2Int.zero;
            return false;
        }
        private bool GetFreePositionWithStackableItem(out Vector2Int position, int itemId, int neededSlotsCount)
        {
            for (int y = Columns - 1; y >= 0; y--)
            {
                for (int x = 0; x < Rows; x++)
                {
                    Vector2Int nextItemPosition = new Vector2Int(x, y);
                    if (Items.ContainsKey(nextItemPosition))
                    {
                        if (CheckItem(Items[nextItemPosition]))
                        {
                            position = nextItemPosition;
                            return true;
                        }
                    }
                }
            }
            for (int x = 0; x < FastSlotsCount; x++)
            {
                Vector2Int nextItemPosition = new Vector2Int(x, -1);
                if (Items.ContainsKey(nextItemPosition))
                {
                    if (CheckItem(Items[nextItemPosition]))
                    {
                        position = nextItemPosition;
                        return true;
                    }
                }
            }

            if (GetFreePosition(out position)) return true;

            position = Vector2Int.zero;
            return false;

            bool CheckItem(IGameItemData item)
            {
                if (item.Id == itemId)
                {
                    int freeSlotsInCell = item.MaxStackCount - item.CurrentCount;
                    if (freeSlotsInCell >= neededSlotsCount) return true;
                }

                return false;
            }
        }
    }
}
