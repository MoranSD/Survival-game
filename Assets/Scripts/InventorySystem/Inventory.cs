using System.Collections.Generic;
using UnityEngine;
using GameItems;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance;

        public Dictionary<Vector2Int, GameItemInventoryData> Items { get; private set; }
        public UIInventory UIInventory { get; private set; }

        public const int Columns = 5;
        public const int Rows = 5;
        public const int FastSlotsCount = 5;

        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Transform _mainCellsContainer;
        [SerializeField] private Transform _fastCellsContainer;
        [SerializeField] private InventoryCell _cellPrefab;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this.gameObject);

            Items = new Dictionary<Vector2Int, GameItemInventoryData>();
            UIInventory = new UIInventory(_mainCanvas, _mainCellsContainer, _fastCellsContainer, _cellPrefab);
            UIInventory.CreateCells();
        }
        private void Start()
        {
            if (TryLoadData())
                UIInventory.UpdateCells();
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
        public void TryMergeCells(InventoryCell dragCell, InventoryCell mergeCell)//тут добавить from to с типом либо вектора, либо €чеек
        {
            /*
             * сначала проверить есть ли в драг вообще предмет
             * потом проверить есть ли предмет в мердже
             */

            if (Items.ContainsKey(dragCell.GridPosition))
            {
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
        }
        public bool TryAddItem(GameItem gameItem)
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
