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
    private IEnumerable<PlayerInput.ActionEvent> _playerMoveInputActions;
    #endregion
    public delegate void OnMouseClick(Vector3 input);
    public static OnMouseClick OnMouseClicked;
    public Vector3 GetInput()
    {
      return new Vector3(_freeMoveVector.x, 0, _freeMoveVector.y);
    }

    public void InitializeMoveInputActions(PlayerInput playerInputs)
    {
      _playerMoveInputActions = playerInputs.actionEvents.Where(u =>
            u.actionName.Contains(Constants.PlayerMoveAction) ||
            u.actionName.Contains(Constants.PlayerClickMoveAction));
      
      var playerMoveInputActions = _playerMoveInputActions.ToList();
      for (int i = 0; i < playerMoveInputActions.Count; i++)
      {
        BindMoveInputCallbacks(playerMoveInputActions[i],i);
      }
    }
    private void BindMoveInputCallbacks(PlayerInput.ActionEvent actionEvent,int index)
    {
      switch (index)
      {
        case 0:
          actionEvent.AddListener(MoveInput);
          break;
        case 1:
          actionEvent.AddListener(MouseClickInput);
          break;
      }
    }

    #region Input-Actions-Callback
    public void MoveInput(InputAction.CallbackContext input)
    {
      _freeMoveVector= input.ReadValue<Vector2>();
    }

    public void MouseClickInput(InputAction.CallbackContext mouseInput)
    {
      _mouseClickVector = Mouse.current.position.ReadValue();
      OnMouseClicked?.Invoke(_mouseClickVector);
    }
    #endregion
  }
}
