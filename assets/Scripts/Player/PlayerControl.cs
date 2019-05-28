using System.Collections.Generic;
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
        [SerializeField] private float JumpForce = 4;
        private const float MinJumpInterval = 0.25f;
        private float _jumpTimeStamp;
        private bool _isGrounded = true;
        private bool _wasGrounded;
        private readonly List<Collider> _collisions = new List<Collider>();

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            _cameraTransform = GameController.Instance.LevelCamera.transform;
        }

        private void Update()
        {
            Move();
            JumpingAndLanding();
        }        

        private void Move()
        {
            float v = PlayerInput.Vertical(playerId);
            float h = PlayerInput.Horizontal(playerId);

            _currentV = Mathf.Lerp(_currentV, v, Time.deltaTime * TurnSensitivity);
            _currentH = Mathf.Lerp(_currentH, h, Time.deltaTime * TurnSensitivity);

            Vector3 direction = _cameraTransform.forward * _currentV + _cameraTransform.right * _currentH;

            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                _currentDirection = Vector3.Lerp(_currentDirection, direction, Time.deltaTime * TurnSensitivity);

                transform.rotation = Quaternion.LookRotation(_currentDirection);
                transform.position += _currentDirection * MoveSpeed * Time.deltaTime;

                _animator.SetFloat("MoveSpeed", direction.magnitude);                
            }
            else
            {
                _animator.SetFloat("MoveSpeed", 0f);
            }
        }

        #region Jumping

        private void JumpingAndLanding()
        {
            //_animator.SetBool("Grounded", _isGrounded);

            var jumpCooldownIsOver = (Time.time - _jumpTimeStamp) >= MinJumpInterval;
            if (jumpCooldownIsOver && _isGrounded && PlayerInput.Jump(playerId))
            {
                _jumpTimeStamp = Time.time;
                _rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }

            //if (!_wasGrounded && _isGrounded)
            //    _animator.SetTrigger("Land");

            //if (!_isGrounded && _wasGrounded)
            //    _animator.SetTrigger("Jump");

            _wasGrounded = _isGrounded;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var contactPoints = collision.contacts;
            foreach (var contact in contactPoints)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    if (!_collisions.Contains(collision.collider))
                        _collisions.Add(collision.collider);

                    _isGrounded = true;
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            var contactPoints = collision.contacts;
            var validSurfaceNormal = false;

            foreach (var contact in contactPoints)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    validSurfaceNormal = true;
                    break;
                }
            }

            if (validSurfaceNormal)
            {
                _isGrounded = true;
                if (!_collisions.Contains(collision.collider))
                    _collisions.Add(collision.collider);
            }
            else
            {
                if (_collisions.Contains(collision.collider))
                    _collisions.Remove(collision.collider);

                if (_collisions.Count == 0)
                    _isGrounded = false;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (_collisions.Contains(collision.collider))
                _collisions.Remove(collision.collider);

            if (_collisions.Count == 0)
                _isGrounded = false;
        }

        #endregion Jumping
    }
}
