using UnityEngine;
using UnityEngine.InputSystem;

namespace Helpers
{
    public static class ActionEventExtension
    {
        public static void SetActionEventName(this PlayerInput.ActionEvent action,string name)
        {
            //action.actionName = name;
        }
    }
}