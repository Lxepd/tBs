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

    protected float animTime;

    protected virtual void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    // 改变朝向
    protected virtual void Rotate(Vector3 dir)
    {

    }
    protected virtual void Rotate(float dir)
    {

    }
}
