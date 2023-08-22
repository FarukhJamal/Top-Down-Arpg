using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerController : MonoBehaviour,IMovement
    {
        #region Variables
        private InputManager _inputManager;
        private Rigidbody _rb;
        private Camera _isometricCamera;
        private PlayerInput _playerInputs;
        private Coroutine _coroutine;
        private int groundLayer;
        
        [Header(" ---- Player Movement ---- ")]
        
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float rotationSpeed = 10f;
        #endregion

        public InputManager InputManager => _inputManager;
        #region Unity-Calls
        private void Awake()
        {
            GameManager.Instance.PlayerController = this;
            _rb = GetComponent<Rigidbody>();
            _inputManager = new InputManager();
            _playerInputs = GetComponent<PlayerInput>();
            groundLayer = LayerMask.NameToLayer($"Ground");
            _isometricCamera=Camera.main;
          
        }
        
        private void OnEnable()
        {
            _inputManager.InitializeMoveInputActions();
            InputManager.OnMouseClicked += ClickToMove;
        }

        private void OnDisable()
        {
            _inputManager.DeleteMoveInputActions();
            InputManager.OnMouseClicked -= ClickToMove;
        }

        void Update()
        {
            Look( _inputManager.GetInput());
        }

        private void FixedUpdate()
        {
            Move( _inputManager.GetInput());
        }
        #endregion

        #region Private-Functions

        private IEnumerator PlayerMoveTowards(Vector3 target)
        {
            var playerDistanceToFloor = transform.position.y - target.y;
            target.y += playerDistanceToFloor;
            
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                Vector3 direction = target - transform.position;
                
                _rb.velocity = direction.normalized * movementSpeed;
                
                transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(direction.normalized),rotationSpeed * Time.deltaTime);
                
                yield return null;
            }
        }
        #endregion
        
        #region Public-Functions

        #region Free-Movement
        public void Move(Vector3 input)
        {
            _rb.MovePosition(transform.position + transform.forward * (input.magnitude * movementSpeed * Time.deltaTime));
        }
        public void Look(Vector3 input)
        {
            if (input == Vector3.zero) return;
            
            var position = transform.position;
            var relative = (position + input.ToIso()) - position;
            var rotation = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        #endregion

        #region Click-To-Move
        
        public void ClickToMove(Vector3 mouseInput)
        {
            var ray=_isometricCamera.ScreenPointToRay(mouseInput);
            if (!Physics.Raycast(ray: ray, hitInfo: out var raycastHit) && !raycastHit.collider && raycastHit.collider.gameObject.layer.CompareTo(groundLayer)==0) 
                return;
            
            if(_coroutine!=null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(PlayerMoveTowards(raycastHit.point));
        }
        #endregion
        
        #endregion
    }
}
