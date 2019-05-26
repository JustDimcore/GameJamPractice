using UnityEngine;

namespace Player
{
    public class PlayerControl : MonoBehaviour
    {
        public PlayerId playerId = PlayerId.First;
        private Animator _animator;
        private Rigidbody _rigidbody;
        private Transform _cameraTransform;

        [Header("Movement")]
        [SerializeField] float TurnSensitivity = 30;
        [SerializeField] private float MoveSpeed = 2;
        private float _currentV;
        private float _currentH;
        private Vector3 _currentDirection = Vector3.zero;

        [Header("Jumping")]
        [SerializeField] private float _jumpForce = 4;
        private const float MinJumpInterval = 0.25f;
        private float _jumpTimeStamp;
        private bool _isGrounded = true;
        private bool _wasGrounded;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            _animator.SetBool("Grounded", _isGrounded);
            Move();
            JumpingAndLanding();
        }

        private void Move()
        {
            float v = InputController.Vertical(playerId);
            float h = InputController.Horizontal(playerId);

            _currentV = Mathf.Lerp(_currentV, v, Time.deltaTime * TurnSensitivity);
            _currentH = Mathf.Lerp(_currentH, h, Time.deltaTime * TurnSensitivity);

            Vector3 direction = _cameraTransform.forward * _currentV + _cameraTransform.right * _currentH;

            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                _currentDirection = Vector3.Slerp(_currentDirection, direction, Time.deltaTime * TurnSensitivity);

                transform.rotation = Quaternion.LookRotation(_currentDirection);
                transform.position += _currentDirection * MoveSpeed * Time.deltaTime;

                _animator.SetFloat("MoveSpeed", direction.magnitude);
            }
        }

        private void JumpingAndLanding()
        {
            var jumpCooldownIsOver = (Time.time - _jumpTimeStamp) >= MinJumpInterval;
            if (jumpCooldownIsOver && _isGrounded && InputController.Jump(playerId))
            {
                _jumpTimeStamp = Time.time;
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            }

            if (!_wasGrounded && _isGrounded)
                _animator.SetTrigger("Land");

            if (!_isGrounded && _wasGrounded)
                _animator.SetTrigger("Jump");

            _wasGrounded = _isGrounded;
        }
    }
}
