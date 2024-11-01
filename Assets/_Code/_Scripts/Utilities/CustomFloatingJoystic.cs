using System;
using UnityEngine.EventSystems;

public class CustomFloatingJoystic : FloatingJoystick {
    public Action PointerUpEvent;
    public Action PointerDownEvent;

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        PointerUpEvent?.Invoke();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        PointerDownEvent?.Invoke();
    }
}
