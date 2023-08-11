using System;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour,IMovement
    {
        private InputManager _inputManager;
        private Rigidbody rb;
        [SerializeField] private float _movementSpeed = 10f;
        [SerializeField] private float _rotationSpeed = 10f;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            _inputManager = new InputManager();
        }

        // Update is called once per frame
        void Update()
        {
            _inputManager.GetInput();
            _inputManager.Look(transform,_rotationSpeed);
        }

        private void FixedUpdate()
        {
            Move(_inputManager.CurrentInput);
        }

        public void Move(Vector3 input)
        {
            rb.MovePosition(transform.position + (transform.forward * input.magnitude) * _movementSpeed * Time.deltaTime);
        }
    }
}
