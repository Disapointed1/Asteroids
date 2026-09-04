using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _handle;

    public Vector2 InputDirection => _inputDirection;

    private Vector2 _inputDirection = Vector2.zero;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_background, eventData.position,
            eventData.pressEventCamera, out position);

        position = Vector2.ClampMagnitude(position, _background.sizeDelta.x / 2f);
        _handle.anchoredPosition = position;

        _inputDirection = position / (_background.sizeDelta.x / 2f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _inputDirection = Vector2.zero;
        _handle.anchoredPosition = Vector2.zero;
    }
}
