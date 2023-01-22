using System.Collections.Generic;
using UnityEngine;
using GameItems;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        /*
         * тут помимо основных данных предмета, можно будет добавить уникальную для каждого
         */
        public Dictionary<Vector2Int, GameItemInventoryData> Items { get; private set; }

        public const int Columns = 5;
        public const int Rows = 5;
        public const int FastSlotsCount = 5;

        [SerializeField] private Transform _mainCellsContainer;
        [SerializeField] private Transform _fastCellsContainer;
        [SerializeField] private InventoryCell _cellPrefab;

        private InventoryCell[] _mainCells;
        private InventoryCell[] _fastCells;

        private void Awake()
        {
            Items = new Dictionary<Vector2Int, GameItemInventoryData>();
            CreateCells();

            if (TryLoadData())
                UpdateCells();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameItem item = Instantiate(GameItemsCollector.Instance.GetItem(1));

                if (TryAddItem(item))
                    Destroy(item.gameObject);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameItem item = Instantiate(GameItemsCollector.Instance.GetItem(0));

                if (TryAddItem(item))
                    Destroy(item.gameObject);
            }
        }
        public bool TryMergeCells()//тут добавить from to с типом либо вектора, либо ячеек
        {
            return false;
        }
        public bool TryAddItem(GameItem gameItem)
        {
            InventoryCell freeCell;
            if (gameItem.BaseData.IsStackable) freeCell = GetFreeCellWithStackableItem(gameItem.BaseData.Id, gameItem.BaseData.currentCount);
            else freeCell = GetFreeCell();

            if (freeCell != null)
            {
                if (freeCell.IsEmpty) Items.Add(freeCell.GridPosition, new GameItemInventoryData(gameItem.BaseData, gameItem.UnicData));
                else Items[freeCell.GridPosition].baseData.currentCount += gameItem.BaseData.currentCount;

                freeCell.Render(Items[freeCell.GridPosition].baseData);
                return true;
            }

            return false;
        }
        private InventoryCell GetFreeCell()
        {
            for (int i = 0; i < _mainCells.Length; i++)
                if (_mainCells[i].IsEmpty) return _mainCells[i];

            for (int i = 0; i < _fastCells.Length; i++)
                if (_fastCells[i].IsEmpty) return _fastCells[i];

            return null;
        }
        private InventoryCell GetFreeCellWithStackableItem(int itemId, int neededSlotsCount)
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
                        BaseGameItemData itemData = Items[cells[i].GridPosition].baseData;
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
        private void UpdateCells()
        {
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

        private void CreateCells()
        {
            _mainCells = new InventoryCell[Columns * Rows];

            int i = 0;
            for (int y = Columns - 1; y >= 0; y--)
            {
                for (int x = 0; x < Rows; x++)
                {
                    InventoryCell cell = Instantiate(_cellPrefab, _mainCellsContainer);
                    cell.GridPosition = new Vector2Int(x, y);
                    _mainCells[i] = cell;
                    i++;
                }
            }

            _fastCells = new InventoryCell[FastSlotsCount];

            for (int x = 0; x < FastSlotsCount; x++)
            {
                InventoryCell cell = Instantiate(_cellPrefab, _fastCellsContainer);
                cell.GridPosition = new Vector2Int(x, -1);//-1 bcz fast slots below main slots
                _fastCells[x] = cell;
            }
        }

        private bool TryLoadData()
        {
            return false;
        }
    }
    public class GameItemInventoryData
    {
        public BaseGameItemData baseData;
        public object unicData;

        public GameItemInventoryData(BaseGameItemData baseData, object unicData)
        {
            this.baseData = baseData;
            this.unicData = unicData;
        }
    }
}
