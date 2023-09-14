using System.Collections.Generic;
using System.Linq;
using Helpers;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Managers
{
  public class InputManager 
  {
    #region Variables
    private Vector2 _freeMoveVector;
    private Vector3 _mouseClickVector;

    #region Input-Actions
    private Controls playerControls;
    #endregion
    private IEnumerable<PlayerInput.ActionEvent> _playerMoveInputActions;
    #endregion
    public delegate void OnMouseClick(Vector3 input);
    public static OnMouseClick OnMouseClicked;

    public Vector3 MouseClickVector
    {
      set => _mouseClickVector=value;
      get => _mouseClickVector;
    }
    public Vector3 GetInput()
    {
      return new Vector3(_freeMoveVector.x, 0f, _freeMoveVector.y);
    }

    public void InitializeMoveInputActions()
    {
      playerControls = new Controls();
      playerControls.Enable();
      
      playerControls.Player.Move.performed += MoveInput;
      playerControls.Player.Move.canceled += CancelMoveInput;
      playerControls.Player.ClickToMove.performed += MouseClickInput;
    }

    public void DeleteMoveInputActions()
    {
      playerControls.Player.Move.performed -= MoveInput;
      playerControls.Player.Move.canceled -= CancelMoveInput;
      playerControls.Player.ClickToMove.performed -= MouseClickInput;
      playerControls.Disable();
    }
    #region Input-Actions-Callback

    private void MoveInput(InputAction.CallbackContext input)
    {
      _freeMoveVector= input.ReadValue<Vector2>();
    }
    private void CancelMoveInput(InputAction.CallbackContext input)
    {
      _freeMoveVector= Vector2.zero;
    }

    private void MouseClickInput(InputAction.CallbackContext mouseInput)
    {
      _mouseClickVector = Mouse.current.position.ReadValue();
      OnMouseClicked?.Invoke(_mouseClickVector);
    }
    #endregion
  }
}
