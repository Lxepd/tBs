using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorBase : MonoBehaviour
{
    // 获取父类的组件信息
    // 刚体组件
    protected Rigidbody2D rg;
    // 动画组件
    protected Animator anim;

    protected virtual void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    // 改变朝向
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
