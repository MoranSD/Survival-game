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
            InventoryCell freeCell = GetFreeCell();

            //тут потом добавить поиск свободной ячейки по типу и количеству
            if(freeCell != null)
            {
                freeCell.Render(gameItem.BaseData);

                Items.Add(freeCell.GridPosition, new GameItemInventoryData(gameItem.BaseData, gameItem.UnicData));

                UpdateCells();
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
        //перезаписывает информацию в ячейках в зависимости от данных в инвентаре
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
