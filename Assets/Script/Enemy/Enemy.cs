using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rg;
    Animator animator;

    private Vector2 nearThrow;
    private Vector2 playerDir;

    Timer forceTimer, moveTimer;
    bool isStart;

    private void Start()
    {
        // TODO: 初始化怪物

        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        forceTimer = new Timer(1f, false, false);
        moveTimer = new Timer(.2f, true, true);
        // 获取<敌人扣血>的消息
        EventCenter.GetInstance().AddEventListener<ThrowItemData>("敌人扣血", (x) =>
        {
            if (nearThrow == Vector2.zero)
                return;

            Debug.Log("敌人扣血：    " + x.hurt);
            Debug.Log(nearThrow);
            animator.Play("Hit");
            // 力 = 投掷物来向 * 投掷物重量
            rg.AddForce(nearThrow * x.mass, ForceMode2D.Impulse);
            //forceTimer.Start();
        });

    }
    private void FixedUpdate()
    {

        FindThrowDir();
        CheckNearPlayer();
        //if(forceTimer.isTimeUp)
        //{

        //}

        if(moveTimer.isTimeUp)
        {
            // 朝玩家移动
            rg.velocity = playerDir * 5;
        }
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
    private void CheckNearPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, 5f, LayerMask.GetMask("玩家"));

        if (player == null)
        {
            playerDir = Vector2.zero;
            return;
        }

        playerDir = (player.transform.position - transform.position).normalized;
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 2f);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 5f);

    }
}
