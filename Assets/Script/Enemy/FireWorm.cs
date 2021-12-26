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
    [Tooltip("������Χ")]
    public float MoveLen = 12f;

    private void Start()
    {
        // TODO: ��ʼ������

        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        forceTimer = new Timer(.2f, false, false);
        moveTimer = new Timer(.5f * GameTool.GetAnimatorLength(animator, "Walk"), false, false);
        // ��ȡ<���˿�Ѫ>����Ϣ
        EventCenter.GetInstance().AddEventListener<ThrowItemData>("���˿�Ѫ", (x) =>
        {
            if (nearThrow == Vector2.zero)
                return;

            Debug.Log("���˿�Ѫ��    " + x.hurt);
            animator.Play("Hit");
            // �� = Ͷ�������� * Ͷ��������
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
    /// Ѱ������Ͷ����
    /// </summary>
    private void FindThrowDir()
    {
        LayerMask mask = LayerMask.GetMask("��������") | LayerMask.GetMask("ǿ����");
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2f, mask);
        if (cols.Length == 0)
            return;

        GameTool.QuickSortArray(transform.position, cols, 0, cols.Length - 1);
        nearThrow = (transform.position - cols[0].transform.position).normalized;
    }
    /// <summary>
    /// Ѱ�ҷ�Χ�ڵ���ң���鹥����Χ
    /// </summary>
    private void CheckMoveRange()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, MoveLen, LayerMask.GetMask("���"));

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
        EventCenter.GetInstance().EventTrigger<Vector2>("���λ��", playerDir);
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
            // �ƶ� = ��ҳ��� * ������������
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
