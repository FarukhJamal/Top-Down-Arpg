using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IMovement
{
    public void Move(Vector3 input);
    public void ClickToMove(Vector3 input);
}
