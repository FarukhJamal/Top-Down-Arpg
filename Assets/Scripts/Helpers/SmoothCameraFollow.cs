using System;
using UnityEngine;

namespace Helpers
{
    public class SmoothCameraFollow : MonoBehaviour
    {
        #region Variables
    
        private Vector3 _offset;
        [SerializeField] private Transform target;
        [SerializeField] private float smoothTime;
        private Vector3 _currentVelocity = Vector3.zero;
        [SerializeField] private Camera mainCam;
        public Camera MainCamera =>mainCam;
        
        #endregion
    
        #region Unity callbacks
    
     //   private void Awake() => _offset = transform.position - target.position;
        
        private void LateUpdate()
        {
            var targetPosition = target.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
        }

        public void SetCameraTarget(Transform _target)
        {
            target = _target;
            _offset = transform.position - target.position;
        }

        #endregion
    }
}