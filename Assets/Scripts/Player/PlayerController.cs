using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using Managers;
using PathFinding;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
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
        private Coroutine _coroutine;
        private int _groundLayer;

        public Camera IsometricCamera
        {
            set => _isometricCamera = value;
            get => _isometricCamera;
        }
        [Header(" ---- Player Movement ---- ")]
        
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float rotationSpeed = 10f;
        private NavMeshPath _path;
        #endregion
        #region Unity-Calls
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _inputManager = new InputManager();
            _groundLayer = LayerMask.NameToLayer($"Ground");
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

        private IEnumerator PlayerMoveTowards(Vector3[] paths)
        {
            foreach (Vector3 path in paths)
            {
                var playerDistanceToFloor = transform.position.y - path.y;
                var updatedPath = new Vector3(path.x, path.y + playerDistanceToFloor, path.z);
                while (Vector3.Distance(transform.position, updatedPath) > 0.1f)
                {
                    Vector3 direction = updatedPath - transform.position;
                
                    _rb.velocity = direction.normalized * movementSpeed;
                
                    transform.rotation=Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(direction.normalized),rotationSpeed * Time.deltaTime);
                
                    yield return null;
                }
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
            if (!Physics.Raycast(ray: ray, hitInfo: out var raycastHit) && raycastHit.collider!=null&& !raycastHit.collider && raycastHit.collider.gameObject.layer.CompareTo(_groundLayer)==0) 
                return;
            
            if (raycastHit.collider != null)
            {
                _path = new NavMeshPath();
                if (NavMesh.CalculatePath(transform.position, raycastHit.point, NavMesh.AllAreas, _path))
                {
                    if (_path.status == NavMeshPathStatus.PathComplete)
                    {
                        Debug.Log("Valid Path");
                        Debug.Log(_path.corners.Length);
                        if (_coroutine != null) StopCoroutine(_coroutine);
                        _coroutine = StartCoroutine(PlayerMoveTowards(_path.corners));
                    }
                }
            }


        }
        #endregion
        #endregion
        
    }
}
