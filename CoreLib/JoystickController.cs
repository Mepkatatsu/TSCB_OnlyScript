using UnityEngine;
using UnityEngine.EventSystems;

namespace CoreLib
{
    public interface IJoystickInputHandler
    {
        public void HandleJoystickInput(Vector2 joystickInput);
    }

    public class JoystickController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Variables

        [SerializeField] private RectTransform joystick;
        [SerializeField] private RectTransform handle;

        private Vector2 _centerPosition;
        private Vector2 _normalizedPosition;
    
        private IJoystickInputHandler _joystickInputHandler;

        private Camera MainCamera => _mainCamera ??= Camera.main;
        private Camera _mainCamera;

        #endregion Variables

        #region Methods

        public void SetJoystickHandler(IJoystickInputHandler handler)
        {
            _joystickInputHandler = handler;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!MainCamera)
                return;
        
            _centerPosition = MainCamera.ScreenToWorldPoint(eventData.position);
            joystick.position = _centerPosition;
            handle.position = _centerPosition;
            joystick.gameObject.SetActive(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_joystickInputHandler == null)
                return;
        
            if (!MainCamera)
                return;
        
            Vector2 eventPosition = MainCamera.ScreenToWorldPoint(eventData.position);

            _normalizedPosition = (eventPosition - _centerPosition).normalized;
            _joystickInputHandler.HandleJoystickInput(_normalizedPosition);

            if (Vector2.Distance(eventPosition, _centerPosition) < 1.3f)
            {
                handle.position = eventPosition;
            }
            else
            {
                handle.position = _centerPosition + (_normalizedPosition * 1.3f);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_joystickInputHandler == null)
                return;
        
            _joystickInputHandler.HandleJoystickInput(Vector2.zero);
            joystick.gameObject.SetActive(false);
        }

        #endregion Methods
    }
}