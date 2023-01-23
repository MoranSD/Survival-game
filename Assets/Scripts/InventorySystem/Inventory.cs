using System.Collections.Generic;
using UnityEngine;
using GameItems;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance;

        public Dictionary<Vector2Int, GameItemInventoryData> Items { get; private set; }

        public const int Columns = 5;
        public const int Rows = 5;
        public const int FastSlotsCount = 5;

        [SerializeField] private UIInventory _uiInventory;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);

            Items = new Dictionary<Vector2Int, GameItemInventoryData>();
        }
        private void Start()
        {
            if (TryLoadData())
                _uiInventory.UpdateCells();
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
            if (gameItem.BaseData.IsStackable) freeCell = _uiInventory.GetFreeCellWithStackableItem(gameItem.BaseData.Id, gameItem.BaseData.currentCount);
            else freeCell = _uiInventory.GetFreeCell();

            if (freeCell != null)
            {
                if (freeCell.IsEmpty) Items.Add(freeCell.GridPosition, new GameItemInventoryData(gameItem.BaseData, gameItem.UnicData));
                else Items[freeCell.GridPosition].baseData.currentCount += gameItem.BaseData.currentCount;

                _uiInventory.UpdateCell(freeCell);
                return true;
            }

            return false;
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
