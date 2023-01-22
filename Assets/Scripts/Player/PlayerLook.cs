using UnityEngine;

namespace Player
{
	public class PlayerLook : MonoBehaviour
	{
		[SerializeField] private float _turnSpeed;
        [SerializeField] private float _lookXLimit;
		[SerializeField] private Transform _faceCameraTF;

        private Vector2 _currentRotation = Vector2.zero;

        private void Update()
        {
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
    }
}
