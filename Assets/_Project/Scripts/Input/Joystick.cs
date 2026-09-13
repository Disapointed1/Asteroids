using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _handle;

    public Vector2 InputDirection { get; private set; } = Vector2.zero;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_background, eventData.position,
            eventData.pressEventCamera, out position);

        var radius = _background.sizeDelta.x / 2f;

        position = Vector2.ClampMagnitude(position, radius);
        _handle.anchoredPosition = position;

        InputDirection = position / radius;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputDirection = Vector2.zero;
        _handle.anchoredPosition = Vector2.zero;
    }
}