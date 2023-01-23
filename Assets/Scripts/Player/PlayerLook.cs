using UnityEngine;

namespace Player
{
	public class PlayerLook : MonoBehaviour
    {
        public bool CanTurn { get; private set; } = true;

        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _lookXLimit;
		[SerializeField] private Transform _faceCameraTF;

        private Vector2 _currentRotation = Vector2.zero;
        private void Awake()
        {
            InventorySystem.Inventory.OnChangeInventoryVisibleStateEvent += OnChangeInventoryState;
        }
        private void OnDisable()
        {
            InventorySystem.Inventory.OnChangeInventoryVisibleStateEvent -= OnChangeInventoryState;
        }
        private void Update()
        {
            if (CanTurn == false) return;

            HandleLookInput();

            UpdateFaceTransform();
        }
        private void HandleLookInput()
        {
            _currentRotation.x += -Input.GetAxis("Mouse Y") * _turnSpeed;
            _currentRotation.y += Input.GetAxis("Mouse X") * _turnSpeed;

            _currentRotation.x = Mathf.Clamp(_currentRotation.x, -_lookXLimit, _lookXLimit);
        }
        private void UpdateFaceTransform()
        {
            _faceCameraTF.localRotation = Quaternion.Euler(_currentRotation.x, 0, 0);
            transform.rotation = Quaternion.Euler(0, _currentRotation.y, 0);
        }
        private void OnChangeInventoryState(bool state) => CanTurn = !state;
    }
}
