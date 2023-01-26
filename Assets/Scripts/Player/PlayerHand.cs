using UnityEngine;
using InventorySystem;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerLook))]
	internal class PlayerHand : MonoBehaviour
	{
        internal Inventory Inventory { get; private set; }

        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private Transform _mainCellsContainer;
        [SerializeField] private Transform _fastCellsContainer;
        [SerializeField] private InventoryCell _cellPrefab;

        private PlayerMovement _playerMovement;
		private PlayerLook _playerLook;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerLook = GetComponent<PlayerLook>();

            Inventory = new Inventory(_mainCanvas, _menuPanel, _mainCellsContainer, _fastCellsContainer, _cellPrefab);
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
        }
    }
}
