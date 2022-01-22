using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : BehaviorBase
{
    protected EnemyData data;
    public EnemyData Data { get => data; }

    public Rigidbody2D Rg { get => rg; }
    public Animator Animator { get => anim; }
    public float AnimaTime { get => nowAnimatorTime; }
    // ����״̬��
    [HideInInspector] protected StateMachine machine;

    // ����ӵ�����
    [HideInInspector] protected Vector2 nearThrow;

    public bool stopMono;

    protected Timer forceStopTimer;
    public Collider2D player;

    protected int id;
    protected bool isHit;
    public bool Hit { get => isHit; set => isHit = value; }

    protected Image hp;
    protected float currentHp;
    protected float nowAnimatorTime;

    // ע��״̬��ʱ��
    public Timer moveTimer, atkTimer, HitTimer, DeadTimer;
    public bool playerIsDeath;

    protected override void Start()
    {
        base.Start();

        data = Datas.GetInstance().EnemyDataDic[id];

        hp = GameTool.FindTheChild(gameObject, "Hp").GetComponent<Image>();
        hp.fillAmount = (currentHp = data.hp) / data.hp;

        // ��ȡ<���˿�Ѫ>����Ϣ
        EventCenter.GetInstance().AddEventListener<float>("���˿�Ѫ", (x) =>
        {
            if (nearThrow == Vector2.zero)
                return;

            currentHp -= (int)x;
            isHit = true;
        });
        EventCenter.GetInstance().AddEventListener("�������", () =>
        {
            playerIsDeath = true;
        });
    }
    protected virtual void Update()
    {
        if (stopMono)
            return;

        nowAnimatorTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        // ״̬ѡ��
        CheckState();
        // �����ҹ���
        FindThrowDir();
        // ����״̬������
        machine.Update();

        hp.fillAmount = Mathf.Lerp(hp.fillAmount, currentHp / data.hp, Time.deltaTime * 10f);
    }
    // �ı䳯��
    protected override void Rotate(Vector3 dir)
    {
        Vector3 playerDir = (dir - transform.position).normalized;
        if (playerDir.x <= 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            hp.fillOrigin = (int)Image.OriginHorizontal.Right;
        }
        else
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            hp.fillOrigin = (int)Image.OriginHorizontal.Left;
        }
    }
    // ״̬�ı�
    public virtual void CheckState()
    {
        player = Physics2D.OverlapCircle(transform.position, data.moveLen, LayerMask.GetMask("���"));

    }
    /// <summary>
    /// Ѱ������Ͷ����
    /// </summary>
    private void FindThrowDir()
    {
        LayerMask mask = LayerMask.GetMask("��������") | LayerMask.GetMask("ǿ������");
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2f, mask);
        if (cols.Length == 0)
        {
            nearThrow = Vector2.zero;
            return;
        }

        GameTool.QuickSortArray(transform.position, cols, 0, cols.Length - 1);
        nearThrow = (transform.position - cols[0].transform.position).normalized;
    }
    private void OnDrawGizmosSelected()
    {
        if (data == null)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, data.hitLen);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, data.moveLen);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, data.atkLen);
    }
}

