using UnityEngine;
using GameItems;
using System.Collections.Generic;

namespace InventorySystem
{
	public class UIInventory
	{
        public Canvas MainCanvas { get; private set; }
        public bool IsVisible { get; private set; } = false;

        private GameObject _panel;
        private Transform _mainCellsContainer;
        private Transform _fastCellsContainer;
        private InventoryCell _cellPrefab;

        private InventoryCell[] _mainCells;
        private InventoryCell[] _fastCells;

        public UIInventory(Canvas mainCanvas, GameObject menuPanel, Transform mainCellsContainer, Transform fastCellsContainer, InventoryCell cellPrefab)
        {
            MainCanvas = mainCanvas;
            _panel = menuPanel;

            _mainCellsContainer = mainCellsContainer;
            _fastCellsContainer = fastCellsContainer;
            _cellPrefab = cellPrefab;
        }
        public void Show()
        {
            _panel.SetActive(true);
            IsVisible = true;
        }
        public void Hide()
        {
            _panel.SetActive(false);
            IsVisible = false;
        }
        public void CreateCells()
        {
            _mainCells = new InventoryCell[Inventory.Columns * Inventory.Rows];

            int i = 0;
            for (int y = Inventory.Columns - 1; y >= 0; y--)
            {
                for (int x = 0; x < Inventory.Rows; x++)
                {
                    InventoryCell cell = Object.Instantiate(_cellPrefab, _mainCellsContainer);
                    cell.GridPosition = new Vector2Int(x, y);
                    _mainCells[i] = cell;
                    i++;
                }
            }

            _fastCells = new InventoryCell[Inventory.FastSlotsCount];

            for (int x = 0; x < Inventory.FastSlotsCount; x++)
            {
                InventoryCell cell = Object.Instantiate(_cellPrefab, _fastCellsContainer);
                cell.GridPosition = new Vector2Int(x, -1);//-1 bcz fast slots below main slots
                _fastCells[x] = cell;
            }
        }
        public void UpdateCells()
        {
            Dictionary<Vector2Int, GameItemInventoryData> Items = Inventory.Instance.Items;

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
        public void UpdateCell(InventoryCell cell)
        {
            if (Inventory.Instance.Items.ContainsKey(cell.GridPosition))
            {
                BaseGameItemData itemData = Inventory.Instance.Items[cell.GridPosition].baseData; 
                cell.Render(itemData);
            }
            else
            {
                cell.ResetRender();
            }
        }
        public InventoryCell GetFreeCell()
        {
            for (int i = 0; i < _mainCells.Length; i++)
                if (_mainCells[i].IsEmpty) return _mainCells[i];

            for (int i = 0; i < _fastCells.Length; i++)
                if (_fastCells[i].IsEmpty) return _fastCells[i];

            return null;
        }
        public InventoryCell GetFreeCellWithStackableItem(int itemId, int neededSlotsCount)
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
                        BaseGameItemData itemData = Inventory.Instance.Items[cells[i].GridPosition].baseData;
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
