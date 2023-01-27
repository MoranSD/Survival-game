using UnityEngine;
using InventorySystem;
using System.Collections.Generic;
using GameItems;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerLook))]
	internal class PlayerHand : MonoBehaviour
	{
        internal Inventory Inventory { get; private set; }

        [SerializeField] private Transform _itemInHandsContainer;
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private Transform _mainCellsContainer;
        [SerializeField] private Transform _fastCellsContainer;
        [SerializeField] private InventoryCell _cellPrefab;

        private PlayerMovement _playerMovement;
		private PlayerLook _playerLook;

        private List<ItemInHands> _itemsInHands;
        private int _activeItem = 0;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerLook = GetComponent<PlayerLook>();

            Inventory = new Inventory(_mainCanvas, _menuPanel, _mainCellsContainer, _fastCellsContainer, _cellPrefab);

            _itemsInHands = new List<ItemInHands>();
            for (int i = 0; i < Inventory.FastSlotsCount; i++)
            {
                ItemInHands itemInHands = Instantiate(new ItemInHands(), _itemInHandsContainer);
                IGameItemData itemInInventory = Inventory.GetItemFromFastSlots(i);
                itemInHands.SetItem(itemInInventory);

                _itemsInHands.Add(itemInHands);
            }

            _itemsInHands[_activeItem].Show();

            Inventory.OnUpdateItemInFastSlots += UpdateItemInHands;
        }
        private void OnDestroy()
        {
            Inventory.OnUpdateItemInFastSlots -= UpdateItemInHands;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (Inventory.UIInventory.IsVisible) Inventory.UIInventory.Hide();
                else Inventory.UIInventory.Show();

                _playerMovement.CanMove = !Inventory.UIInventory.IsVisible;
                _playerLook.CanTurn = !Inventory.UIInventory.IsVisible;
            }

            float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
            if(mouseWheel != 0 && _itemsInHands[_activeItem].IsActive)
            {
                int oldItem = _activeItem;

                _activeItem += Mathf.RoundToInt(mouseWheel * 10);

                if (_activeItem < 0) _activeItem = Inventory.FastSlotsCount - 1;
                else if (_activeItem >= Inventory.FastSlotsCount) _activeItem = 0;

                _itemsInHands[oldItem].Hide(_itemsInHands[_activeItem].Show);
            }

            _itemsInHands[_activeItem].Interact();
        }
        private void UpdateItemInHands(int id)
        {
            _itemsInHands[id].SetItem(Inventory.GetItemFromFastSlots(id));
        }
    }
}
