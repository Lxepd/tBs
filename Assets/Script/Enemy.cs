using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rg;

    // 需要扣多少血
    private float hurtHpNum;
    private Vector2 nearThrow;
    private void Start()
    {
        // TODO: 初始化怪物

        rg = GetComponent<Rigidbody2D>();
        // 获取<敌人扣血>的消息
        EventCenter.GetInstance().AddEventListener<float>("敌人扣血", (x) =>
        {
            if (nearThrow == Vector2.zero)
                return;

            Debug.Log("敌人扣血：    " + x);
            // 力 = 投掷物来向 * 投掷物重量
            // rg.AddForce(nearThrow * newnearThrow.gameObject.data.mass;
            rg.AddForce(nearThrow * 5, ForceMode2D.Impulse);
        });
    }
    private void Update()
    {
        FindThrowDir();
    }
    /// <summary>
    /// 寻找来向投掷物
    /// </summary>
    private void FindThrowDir()
    {
        LayerMask mask = LayerMask.GetMask("基础投掷物") | LayerMask.GetMask("场景投掷物");
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2f, mask);
        if (cols.Length == 0)
            return;

        GameTool.QuickSortArray(transform.position, cols, 0, cols.Length - 1);
        nearThrow = (transform.position - cols[0].transform.position).normalized;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
