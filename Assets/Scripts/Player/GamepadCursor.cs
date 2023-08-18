using System;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.Serialization;

namespace Player
{
    public class GamepadCursor : MonoBehaviour
    {
       [SerializeField] private PlayerInput playerInput;
       private Mouse _virtualMouse;

       private void OnEnable()
       {
           if (_virtualMouse == null)
               _virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
           else if (!_virtualMouse.added)
               InputSystem.AddDevice(_virtualMouse);
       }
    }
}
