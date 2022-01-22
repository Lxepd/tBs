using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorBase : MonoBehaviour
{
    // ��ȡ����������Ϣ
    // �������
    protected Rigidbody2D rg;
    // �������
    protected Animator anim;

    protected float animTime;

    protected virtual void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    // �ı䳯��
    protected virtual void Rotate(Vector3 dir)
    {

    }
    protected virtual void Rotate(float dir)
    {

    }
}
