using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // 刚体组件
    [HideInInspector] protected Rigidbody2D rg;
    // 动画组件
    [HideInInspector] protected Animator animator;
    // 有限状态机
    [HideInInspector] protected StateMachine machine;

    // 玩家子弹来向
    [HideInInspector] protected Vector2 nearThrow;

    protected Timer forceStopTimer;
    public Collider2D player;
    protected virtual void Start()
    {
        // TODO: 初始化怪物

        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        
    }
    protected virtual void Update()
    {
        // 状态选择
        CheckState();
        // 检查玩家攻击
        FindThrowDir();
        // 有限状态机更新
        machine.Update();
    }
    // 改变朝向
    public void Rotate(Vector2 dir)
    {
        if (dir.x <= 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
    }
    // 状态改变
    public virtual void CheckState()
    {

    }
    /// <summary>
    /// 寻找来向投掷物
    /// </summary>
    private void FindThrowDir()
    {
        LayerMask mask = LayerMask.GetMask("基础攻击") | LayerMask.GetMask("强化物");
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2f, mask);
        if (cols.Length == 0)
            return;

        GameTool.QuickSortArray(transform.position, cols, 0, cols.Length - 1);
        nearThrow = (transform.position - cols[0].transform.position).normalized;
    }
}

