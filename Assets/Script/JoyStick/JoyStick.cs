using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform center;
    public RectTransform bound;

    public float maxRange;
    public static Vector2 dirVector2;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("开始拖拽");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPos;
        float r;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(bound, eventData.position, eventData.pressEventCamera, out localPos);
        // 长度限制
        r = Mathf.Clamp(localPos.magnitude, 0f, maxRange);

        dirVector2 = localPos.normalized;
        localPos = r * dirVector2;
        center.localPosition = localPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        center.localPosition = Vector2.zero;
        dirVector2 = Vector2.zero;
    }
}
