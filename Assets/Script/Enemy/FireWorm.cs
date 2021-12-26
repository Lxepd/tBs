using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorm : MonoBehaviour
{
    private Rigidbody2D rg;
    private Animator animator;

    private Vector2 nearThrow;
    private Vector2 playerDir;

    Timer forceTimer, moveTimer;
    [Tooltip("攻击范围")]
    public float MoveLen = 12f;

    private void Start()
    {
        // TODO: 初始化怪物

        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        forceTimer = new Timer(.2f, false, false);
        moveTimer = new Timer(.5f * GameTool.GetAnimatorLength(animator, "Walk"), false, false);
        // 获取<敌人扣血>的消息
        EventCenter.GetInstance().AddEventListener<ThrowItemData>("敌人扣血", (x) =>
        {
            if (nearThrow == Vector2.zero)
                return;

            Debug.Log("敌人扣血：    " + x.hurt);
            animator.Play("Hit");
            // 力 = 投掷物来向 * 投掷物重量
            rg.AddForce(nearThrow * x.mass, ForceMode2D.Impulse);
            forceTimer.Start();
        });

    }
    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > GameTool.GetAnimatorLength(animator, "Walk") && animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            moveTimer.Reset(.5f * GameTool.GetAnimatorLength(animator, "Walk"));
            rg.velocity = Vector2.zero;
            animator.Play("Idle");
        }

        TimerCheck();
    }

    private void FixedUpdate()
    {
        FindThrowDir();
        CheckMoveRange();
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
    /// <summary>
    /// 寻找范围内的玩家，检查攻击范围
    /// </summary>
    private void CheckMoveRange()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, MoveLen, LayerMask.GetMask("玩家"));

        if (player == null)
        {
            moveTimer.Reset(.5f * GameTool.GetAnimatorLength(animator, "Walk"), true);
            return;
        }

        if (moveTimer.isStop)
        {
            animator.Play("Walk");
            moveTimer.Start();
        }

        playerDir = (player.transform.position - transform.position).normalized;
        EventCenter.GetInstance().EventTrigger<Vector2>("玩家位置", playerDir);
    }
    private void Rotate()
    {
        if (playerDir.x < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
    }

    private void TimerCheck()
    {
        if (moveTimer.isTimeUp)
        {
            Rotate();
            // 移动 = 玩家朝向 * 怪物自身移速
            rg.velocity = playerDir * 5;
        }
        if (forceTimer.isTimeUp)
        {
            rg.velocity = Vector2.zero;
            moveTimer.Reset(.5f * GameTool.GetAnimatorLength(animator, "Walk"), true);
            forceTimer.Reset(.2f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 2f);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, MoveLen);
    }
}
