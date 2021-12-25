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
        // TODO: ��ʼ������

        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        forceTimer = new Timer(1f, false, false);
        moveTimer = new Timer(.2f, true, true);
        // ��ȡ<���˿�Ѫ>����Ϣ
        EventCenter.GetInstance().AddEventListener<ThrowItemData>("���˿�Ѫ", (x) =>
        {
            if (nearThrow == Vector2.zero)
                return;

            Debug.Log("���˿�Ѫ��    " + x.hurt);
            Debug.Log(nearThrow);
            animator.Play("Hit");
            // �� = Ͷ�������� * Ͷ��������
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
            // ������ƶ�
            rg.velocity = playerDir * 5;
        }
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
    private void CheckNearPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, 5f, LayerMask.GetMask("���"));

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
