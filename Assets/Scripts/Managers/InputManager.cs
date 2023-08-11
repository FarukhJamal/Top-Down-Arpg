using Helpers;
using UnityEngine;

namespace Managers
{
  public class InputManager 
  {
    #region Variables
    private Vector3 _input;
    public Vector3 CurrentInput => _input;
    #endregion
  
    public void GetInput()
    {
      _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    public void Look(Transform playerTransform,float turnSpeed)
    {
      if (_input != Vector3.zero)
      {
        var relative = (playerTransform.position + _input.ToIso()) - playerTransform.position;
        var rotation = Quaternion.LookRotation(relative, Vector3.up);

        playerTransform.rotation =
          Quaternion.RotateTowards(playerTransform.rotation, rotation, turnSpeed * Time.deltaTime);
      }
    }
  }
}
