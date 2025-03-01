using SingletonPattern;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Variables

    [SerializeField] private RectTransform joystick;
    [SerializeField] private RectTransform handle;
    private ShootingGameManager _shootingGameManager;

    private Vector2 _centerPosition;
    private Vector2 _normalizedPosition;

    #endregion Variables

    #region Methods

    private void Start()
    {
        _shootingGameManager = ShootingGameManager.Instance;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Camera.main == null)
            return;
        
        _centerPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        joystick.position = _centerPosition;
        handle.position = _centerPosition;
        joystick.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Camera.main == null)
            return;
        
        Vector2 eventPosition = Camera.main.ScreenToWorldPoint(eventData.position);

        _normalizedPosition = (eventPosition - _centerPosition).normalized;
        _shootingGameManager.SetJoystickInput(_normalizedPosition.x, _normalizedPosition.y);

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
        _shootingGameManager.SetJoystickInput(0, 0);
        joystick.gameObject.SetActive(false);
    }

    #endregion Methods
}
