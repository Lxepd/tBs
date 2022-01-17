using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImgClickBase : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    // ���
    protected BoxCollider2D boxCol;
    protected Image img;

    protected virtual void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        img = GetComponent<Image>();
    }
    /// <summary>
    /// �ƶ��˵��
    /// </summary>
    /// <param name="act">�޷���ֵί��</param>
    protected void PhoneTouch(Action act)
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (boxCol.OverlapPoint(Input.GetTouch(0).position))
            {
                act();
            }
        }
    }
    /// <summary>
    /// PC�˵��
    /// </summary>
    /// <param name="act">�޷���ֵί��</param>
    protected void PCMouseDown(Action act)
    {
        if (Input.GetMouseButtonDown(0) && boxCol.OverlapPoint(Input.mousePosition))
        {
            act();
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {

    }
}
