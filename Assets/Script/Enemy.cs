using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rg;

    // ��Ҫ�۶���Ѫ
    private float hurtHpNum;
    private Vector2 nearThrow;
    private void Start()
    {
        // TODO: ��ʼ������

        rg = GetComponent<Rigidbody2D>();
        // ��ȡ<���˿�Ѫ>����Ϣ
        EventCenter.GetInstance().AddEventListener<float>("���˿�Ѫ", (x) =>
        {
            if (nearThrow == Vector2.zero)
                return;

            Debug.Log("���˿�Ѫ��    " + x);
            // �� = Ͷ�������� * Ͷ��������
            // rg.AddForce(nearThrow * newnearThrow.gameObject.data.mass;
            rg.AddForce(nearThrow * 5, ForceMode2D.Impulse);
        });
    }
    private void Update()
    {
        FindThrowDir();
    }
    /// <summary>
    /// Ѱ������Ͷ����
    /// </summary>
    private void FindThrowDir()
    {
        LayerMask mask = LayerMask.GetMask("����Ͷ����") | LayerMask.GetMask("����Ͷ����");
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
