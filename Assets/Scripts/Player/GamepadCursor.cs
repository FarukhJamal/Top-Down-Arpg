using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

namespace Player
{
    public class GamepadCursor : MonoBehaviour
    {
        private PlayerInput _playerInput; 
        private RectTransform _cursorTransform; 
        private Canvas _canvas; 
        private RectTransform _canvasRectTransform;
        private Controls _controls;
        private Mouse _virtualMouse;
        private Mouse _currentMouse;
        private Camera _mainCamera;
        private bool _previousMouseState;
       [SerializeField] private float cursorSpeed = 1000f;
       [SerializeField] private float padding = 50f;

       private string _previousControlScheme;
       private const string GamepadScheme = "Gamepad";
       private const string MouseScheme = "Mouse";
       private void Awake()
       {
           _playerInput = GetComponent<PlayerInput>();
           _controls = new Controls();
           _canvas = UI_Manager.Instance.GameCanvas;
           _canvasRectTransform = UI_Manager.Instance.CanvasRect;
           _cursorTransform = UI_Manager.Instance.CursorRectTransform;
           _mainCamera=Camera.main;
           
       }

       private void OnEnable()
       {
           _currentMouse=Mouse.current;
           
           
           InputDevice virtualMouseInputDevice = InputSystem.GetDevice("VirtualMouse");
           if (virtualMouseInputDevice == null)
               _virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
           else if (!virtualMouseInputDevice.added)
               InputSystem.AddDevice(_virtualMouse);
           else
               _virtualMouse = (Mouse)virtualMouseInputDevice;

           if (_controls != null) _controls.Enable();
           //Pair the device to the user to use the PlayerInput component with the Event System & the Virtual Mouse.
           InputUser.PerformPairingWithDevice(_virtualMouse, _playerInput.user);

           if (_cursorTransform != null)
           {
               Vector2 position = _cursorTransform.anchoredPosition;
               InputState.Change(_virtualMouse.position,position);
           }

           InputSystem.onAfterUpdate += UpdateMotion;
           _playerInput.onControlsChanged += OnControlsChanged;
       }
       private void OnDisable()
       {
           InputSystem.onAfterUpdate -= UpdateMotion;
           _playerInput.onControlsChanged -= OnControlsChanged;
           InputSystem.RemoveDevice(_virtualMouse);
       }
       private void OnControlsChanged(PlayerInput obj)
       {
           if (_playerInput.currentControlScheme == MouseScheme && _previousControlScheme!=MouseScheme)
           {
               _cursorTransform.gameObject.SetActive(false);
               Cursor.visible = true;
               _currentMouse.WarpCursorPosition(_virtualMouse.position.ReadValue());
               _previousControlScheme = MouseScheme;
           }
           else if (_playerInput.currentControlScheme == GamepadScheme && _previousControlScheme != GamepadScheme)
           {
               _cursorTransform.gameObject.SetActive(true);
               Cursor.visible = false;
               InputState.Change(_virtualMouse.position,_currentMouse.position.ReadValue());
               AnchorCursor(_currentMouse.position.ReadValue());
               _previousControlScheme = GamepadScheme;
           }
       }

       private void UpdateMotion()
       {
           if(_virtualMouse==null ||Gamepad.current==null) return;
           
           //Delta
           Vector2 deltaValue = _controls.Player.VirtualCursor.ReadValue<Vector2>();// Gamepad.current.leftStick.ReadValue();
           deltaValue *= cursorSpeed * Time.deltaTime;

           Vector2 currentPosition = _virtualMouse.position.ReadValue();
           Vector2 newPosition = currentPosition + deltaValue;

           newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width-padding); 
           newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height-padding);
           
           InputState.Change(_virtualMouse.position,newPosition);
           InputState.Change(_virtualMouse.delta,deltaValue);
           
           bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();
           if (_previousMouseState !=aButtonIsPressed)
           {
               _virtualMouse.CopyState<MouseState>(out var mouseState);
               mouseState.WithButton(MouseButton.Left, Gamepad.current.aButton.IsPressed());
               InputState.Change(_virtualMouse,mouseState);
               _previousMouseState = aButtonIsPressed;
           }

           AnchorCursor(newPosition);
       }

       private void AnchorCursor(Vector2 position)
       {
           RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, position,
               _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _mainCamera, out var anchoredPosition);
           _cursorTransform.anchoredPosition = anchoredPosition;
       }
    }
}
