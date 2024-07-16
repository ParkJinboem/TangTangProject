using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Joystick : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] Image background;
    [SerializeField] Image handler;

    float joystickRadius;
    Vector2 touchPos;
    Vector2 _moveDir;

    // Start is called before the first frame update
    void Start()
    {
        joystickRadius = background.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        background.transform.position = eventData.position;
        handler.transform.position = eventData.position;
        touchPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handler.transform.position = touchPos;
        _moveDir = Vector2.zero;
        Managers.Game.MoveDir = _moveDir;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchDir = (eventData.position - touchPos);
        float moveDist = Mathf.Min(touchDir.magnitude, joystickRadius);
        _moveDir = touchDir.normalized;
        Vector2 newPos = touchPos + _moveDir * moveDist;
        handler.transform.position = newPos;
        Managers.Game.MoveDir = _moveDir;
    }
}
