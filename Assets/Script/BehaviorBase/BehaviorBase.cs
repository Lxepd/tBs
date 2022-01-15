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

    protected virtual void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    // �ı䳯��
    protected virtual void Rotate(Vector3 dir)
    {
        //if (dir.x < 0)
        //{
        //    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        //}
        //else if (dir.x > 0)
        //{
        //    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        //}
    }
    protected virtual void Rotate(float dir)
    {

    }
}
