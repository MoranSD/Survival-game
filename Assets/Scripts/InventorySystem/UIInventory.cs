using UnityEngine;
using GameItems;
using System.Collections.Generic;

namespace InventorySystem
{
    internal class UIInventory
	{
        internal Canvas MainCanvas { get; private set; }
        internal bool IsVisible { get; private set; } = false;

        private GameObject _panel;
        private Transform _mainCellsContainer;
        private Transform _fastCellsContainer;
        private InventoryCell _cellPrefab;

        private InventoryCell[] _mainCells;
        private InventoryCell[] _fastCells;

        private Inventory _inventory;

        internal UIInventory(Canvas mainCanvas, GameObject menuPanel, Transform mainCellsContainer, Transform fastCellsContainer, InventoryCell cellPrefab, Inventory inventory)
        {
            MainCanvas = mainCanvas;
            _panel = menuPanel;

            _mainCellsContainer = mainCellsContainer;
            _fastCellsContainer = fastCellsContainer;
            _cellPrefab = cellPrefab;

            _inventory = inventory;
        }
        internal void Show()
        {
            _panel.SetActive(true);
            IsVisible = true;
        }
        internal void Hide()
        {
            _panel.SetActive(false);
            IsVisible = false;
        }
        internal void CreateCells()
        {
            _mainCells = new InventoryCell[_inventory.Columns * _inventory.Rows];

            int i = 0;
            for (int y = _inventory.Columns - 1; y >= 0; y--)
            {
                for (int x = 0; x < _inventory.Rows; x++)
                {
                    InventoryCell cell = Object.Instantiate(_cellPrefab, _mainCellsContainer);
                    cell.GridPosition = new Vector2Int(x, y);
                    cell.Inventory = _inventory;
                    _mainCells[i] = cell;
                    i++;
                }
            }

            _fastCells = new InventoryCell[_inventory.FastSlotsCount];

            for (int x = 0; x < _inventory.FastSlotsCount; x++)
            {
                InventoryCell cell = Object.Instantiate(_cellPrefab, _fastCellsContainer);
                cell.GridPosition = new Vector2Int(x, -1);//-1 bcz fast slots below main slots
                cell.Inventory = _inventory;
                _fastCells[x] = cell;
            }
        }
        internal void UpdateCells()
        {
            Dictionary<Vector2Int, GameItemInventoryData> Items = _inventory.Items;

            for (int i = 0; i < _mainCells.Length; i++)
            {
                if (Items.ContainsKey(_mainCells[i].GridPosition))
                {
                    BaseGameItemData nextItemData = Items[_mainCells[i].GridPosition].baseData;
                    _mainCells[i].Render(nextItemData);
                }
                else
                {
                    _mainCells[i].ResetRender();
                }
            }
            for (int i = 0; i < _fastCells.Length; i++)
            {
                if (Items.ContainsKey(_fastCells[i].GridPosition))
                {
                    BaseGameItemData nextItemData = Items[_fastCells[i].GridPosition].baseData;
                    _fastCells[i].Render(nextItemData);
                }
                else
                {
                    _fastCells[i].ResetRender();
                }
            }
        }
        internal void UpdateCell(InventoryCell cell)
        {
            if (_inventory.Items.ContainsKey(cell.GridPosition))
            {
                BaseGameItemData itemData = _inventory.Items[cell.GridPosition].baseData; 
                cell.Render(itemData);
            }
            else
            {
                cell.ResetRender();
            }
        }
        internal InventoryCell GetFreeCell()
        {
            for (int i = 0; i < _mainCells.Length; i++)
                if (_mainCells[i].IsEmpty) return _mainCells[i];

            for (int i = 0; i < _fastCells.Length; i++)
                if (_fastCells[i].IsEmpty) return _fastCells[i];

            return null;
        }
        internal InventoryCell GetFreeCellWithStackableItem(int itemId, int neededSlotsCount)
        {
            if (CheckCells(_mainCells, out InventoryCell mainCell)) return mainCell;
            if (CheckCells(_fastCells, out InventoryCell fastCell)) return fastCell;
            return GetFreeCell();



            bool CheckCells(InventoryCell[] cells, out InventoryCell returnCell)
            {
                for (int i = 0; i < cells.Length; i++)
                {
                    if (cells[i].IsEmpty == false)
                    {
                        BaseGameItemData itemData = _inventory.Items[cells[i].GridPosition].baseData;
                        if (itemData.Id == itemId)
                        {
                            int freeSlotsInCell = itemData.maxStackCount - itemData.currentCount;
                            if (freeSlotsInCell >= neededSlotsCount)
                            {
                                returnCell = cells[i];
                                return true;
                            }
                        }
                    }
                }

                returnCell = null;
                return false;
            }
        }
    }
}
