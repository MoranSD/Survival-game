using System.Collections.Generic;
using UnityEngine;
using GameItems;
using Player;

namespace InventorySystem
{
    internal class Inventory
    {
        internal Dictionary<Vector2Int, GameItemInventoryData> Items { get; private set; }
        internal UIInventory UIInventory { get; private set; }

        internal int Columns = 5;
        internal int Rows = 5;
        internal int FastSlotsCount = 5;

        internal Inventory(Canvas mainCanvas, GameObject menuPanel, Transform mainCellsContainer, Transform fastCellsContainer, InventoryCell cellPrefab, int columns = 5, int rows = 5, int fastSlotsCount = 5)
        {
            Columns = columns;
            Rows = rows;
            FastSlotsCount = fastSlotsCount;

            Items = new Dictionary<Vector2Int, GameItemInventoryData>();
            UIInventory = new UIInventory(mainCanvas, menuPanel, mainCellsContainer, fastCellsContainer, cellPrefab, this);
            UIInventory.Hide();
            UIInventory.CreateCells();
        }
        internal void TryMergeCells(InventoryCell dragCell, InventoryCell mergeCell)//тут добавить from to с типом либо вектора, либо ячеек
        {
            if (Items.ContainsKey(dragCell.GridPosition) == false)
                throw new System.Exception("You trying to drag empty cell");

            GameItemInventoryData dragItemData = Items[dragCell.GridPosition];

            if (Items.ContainsKey(mergeCell.GridPosition))
            {
                GameItemInventoryData mergeItemData = Items[mergeCell.GridPosition];

                if (dragItemData.baseData.Id == mergeItemData.baseData.Id && dragItemData.baseData.IsStackable)
                {
                    int freeSlotsCount = mergeItemData.baseData.maxStackCount - mergeItemData.baseData.currentCount;
                    if (freeSlotsCount >= dragItemData.baseData.currentCount)
                    {
                        Items[mergeCell.GridPosition].baseData.currentCount += Items[dragCell.GridPosition].baseData.currentCount;
                        Items.Remove(dragCell.GridPosition);
                    }
                }
            }
            else
            {
                Items.Add(mergeCell.GridPosition, dragItemData);
                Items.Remove(dragCell.GridPosition);
            }

            UIInventory.UpdateCell(dragCell);
            UIInventory.UpdateCell(mergeCell);
        }
        internal bool TryAddItem(GameItem gameItem)
        {
            InventoryCell freeCell;
            if (gameItem.BaseData.IsStackable) freeCell = UIInventory.GetFreeCellWithStackableItem(gameItem.BaseData.Id, gameItem.BaseData.currentCount);
            else freeCell = UIInventory.GetFreeCell();

            if (freeCell != null)
            {
                if (freeCell.IsEmpty) Items.Add(freeCell.GridPosition, new GameItemInventoryData(gameItem.BaseData, gameItem.UnicData));
                else Items[freeCell.GridPosition].baseData.currentCount += gameItem.BaseData.currentCount;

                UIInventory.UpdateCell(freeCell);
                return true;
            }

            return false;
        }

        internal bool TryLoadData(Dictionary<Vector2Int, GameItemInventoryData> data)
        {
            return false;
        }
    }
}
