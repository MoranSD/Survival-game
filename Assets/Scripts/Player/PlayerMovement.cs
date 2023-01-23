using UnityEngine;

namespace Player
{
	[RequireComponent(typeof(CharacterController))]
	public class PlayerMovement : MonoBehaviour
    {
        public bool CanMove { get; private set; } = true;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private Transform _faceCameraTF;

        private CharacterController _characterController;

        private float _gravity = 0;
        private const float gravityOnGround = 0.1f;
        private const float gravityInSpace = -29.43f;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            InventorySystem.Inventory.OnChangeInventoryVisibleStateEvent += OnChangeInventoryState;
        }
        private void OnDisable()
        {
            InventorySystem.Inventory.OnChangeInventoryVisibleStateEvent -= OnChangeInventoryState;
        }
        private void Update()
        {
            if (CanMove == false) return;

            Vector3 moveDirection = GetMoveDirection();
            SetPhysicsVelocity(ref moveDirection.y);

            _characterController.Move(moveDirection);
        }
        private Vector3 GetMoveDirection()
        {
            Vector3 moveDirection = _faceCameraTF.forward * Input.GetAxisRaw("Vertical") + _faceCameraTF.right * Input.GetAxisRaw("Horizontal");
            moveDirection.y = 0;
            moveDirection.Normalize();
            moveDirection *= _moveSpeed * Time.deltaTime;

            return moveDirection;
        }
        private void SetPhysicsVelocity(ref float value)
        {
            if (_characterController.isGrounded) _gravity = -gravityOnGround;
            else _gravity += gravityInSpace * Time.deltaTime;

            if (_characterController.isGrounded && Input.GetKey(KeyCode.Space))
                _gravity = Mathf.Sqrt(_jumpForce * -2f * gravityInSpace);

            value = _gravity * Time.deltaTime;
        }
        private void OnChangeInventoryState(bool state) => CanMove = !state;
    }
}
