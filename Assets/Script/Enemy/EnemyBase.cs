using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    protected EnemyData data;
    // �������
    [HideInInspector] protected Rigidbody2D rg;
    // �������
    [HideInInspector] protected Animator animator;
    // ����״̬��
    [HideInInspector] protected StateMachine machine;

    // ����ӵ�����
    [HideInInspector] protected Vector2 nearThrow;

    protected Timer forceStopTimer;
    public Collider2D player;

    protected int id;
    protected Image hp;
    protected float currentHp;
    protected virtual void Start()
    {
        // TODO: ��ʼ������

        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        hp = GameTool.FindTheChild(gameObject, "Hp").GetComponent<Image>();

        data = GameTool.GetDicInfo(Datas.GetInstance().EnemyDataDic, id);
        hp.fillAmount = (currentHp = data.hp) / data.hp;
    }
    protected virtual void Update()
    {
        // ״̬ѡ��
        CheckState();
        // �����ҹ���
        FindThrowDir();
        // ����״̬������
        machine.Update();

        hp.fillAmount = Mathf.Lerp(hp.fillAmount, currentHp / data.hp, Time.deltaTime * 10f);
    }
    // �ı䳯��
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
    // ״̬�ı�
    public virtual void CheckState()
    {

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
}

