using UnityEngine;
using GameItems;
using System.Collections.Generic;
using System.Linq;

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

        private Dictionary<Vector2Int, InventoryCell> _mainCells;
        private Dictionary<Vector2Int, InventoryCell> _fastCells;

        internal UIInventory(Canvas mainCanvas, GameObject menuPanel, Transform mainCellsContainer, Transform fastCellsContainer, InventoryCell cellPrefab)
        {
            MainCanvas = mainCanvas;
            _panel = menuPanel;

            _mainCellsContainer = mainCellsContainer;
            _fastCellsContainer = fastCellsContainer;
            _cellPrefab = cellPrefab;
        }
        internal UIInventory(Canvas mainCanvas, GameObject menuPanel, Transform mainCellsContainer, InventoryCell cellPrefab)
        {
            MainCanvas = mainCanvas;
            _panel = menuPanel;

            _mainCellsContainer = mainCellsContainer;
            _cellPrefab = cellPrefab;
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
        internal void CreateCells(Inventory inventory)
        {
            _mainCells = new Dictionary<Vector2Int, InventoryCell>();

            int i = 0;
            for (int y = inventory.Columns - 1; y >= 0; y--)
            {
                for (int x = 0; x < inventory.Rows; x++)
                {
                    InventoryCell cell = Object.Instantiate(_cellPrefab, _mainCellsContainer);
                    cell.GridPosition = new Vector2Int(x, y);
                    cell.Inventory = inventory;
                    _mainCells[cell.GridPosition] = cell;
                    i++;
                }
            }

            if(inventory.FastSlotsCount > 0)
            {
                _fastCells = new Dictionary<Vector2Int, InventoryCell>();

                for (int x = 0; x < inventory.FastSlotsCount; x++)
                {
                    InventoryCell cell = Object.Instantiate(_cellPrefab, _fastCellsContainer);
                    cell.GridPosition = new Vector2Int(x, -1);//-1 bcz fast slots below main slots
                    cell.Inventory = inventory;
                    _fastCells[cell.GridPosition] = cell;
                }
            }
        }
        internal void UpdateCell(Vector2Int cellPosition, IGameItemData itemData)
        {
            InventoryCell targetCell;
            if (cellPosition.y == -1) targetCell = _fastCells[cellPosition];
            else targetCell = _mainCells[cellPosition];


            if (itemData != null) targetCell.Render(itemData);
            else targetCell.ResetRender();
        }
    }
}
