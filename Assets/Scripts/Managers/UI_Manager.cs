using System;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class UI_Manager : MonoBehaviour
    {
        #region Private-Variables
        private Canvas _canvas;
        private RectTransform _canvasRectTransform;
        private RectTransform _cursorRect;
        private PlayerInput _gamepadControls;
        private static string _path = "Prefabs/Controls/";
        #endregion

        #region Public-Variables
        public static UI_Manager Instance;
        public Canvas GameCanvas => _canvas;
        public RectTransform CanvasRect=>_canvasRectTransform;
        public RectTransform CursorRectTransform => _cursorRect;
        public PlayerInput GamepadControls => _gamepadControls;
        #endregion
        
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            // Canvas
            _canvas =GameObject.FindWithTag("Parent").GetComponent<Canvas>();
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
          
        }

        private void Start()
        {

#if UNITY_IPHONE || UNITY_ANDROID
           InstantiateMobileControls();
#endif
            InstantiateGamepadCursor();
        }

        public void InstantiateMobileControls()
        {
            var controlsPrefab=Resources.Load(_path + "MobileControls");
            if (controlsPrefab != null)
            {
                Instantiate(controlsPrefab, _canvas.transform);
            }
        }

        public void InstantiateGamepadCursor()
        {
            var cursor = Resources.Load(_path + "Cursor");
            if (cursor != null)
            {
                var obj = Instantiate(cursor, _canvas.transform);
                if (obj != null)
                    _cursorRect = obj.GetComponent<RectTransform>();
            }
            var gamepadInput=Resources.Load(_path + "GamepadCursor");
            if (gamepadInput != null)
            {
                var control=Instantiate(gamepadInput)as GameObject;
                if (control != null)
                    _gamepadControls = control.GetComponent<PlayerInput>();
            }
        }
    }
}
