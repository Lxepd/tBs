using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BehaviorBase
{
    private Vector2 dir;
    [SerializeField] public PlayerData data;
    [Tooltip("��������Ͷ����ķ�Χ")]
    public float shootThrowThingLen = .5f;
    [Tooltip("�����������˵ķ�Χ")]
    public float shootEnemyLen = 3f;


    protected override void Start()
    {
        base.Start();

        EventCenter.GetInstance().AddEventListener<Vector2>("Joystick", (x) => 
        {
            dir = x;
            Rotate(x);
        });
        EventCenter.GetInstance().EventTrigger<PlayerData>("��ɫ��ʼ", data);
    }
    private void Update()
    {
        rg.velocity = dir * data.speed;

        if (rg.velocity ==Vector2.zero)
        {
            anim.Play("Idle");
        }
        else
        {
            anim.Play("Run");
        }
    }
    private void FixedUpdate()
    {
        Checkstrengthen();
        FindProximityOfEnemy();
        CheckNpcHere();
    }
    /// <summary>
    /// ��鸽����û��ǿ����
    /// </summary>
    private void Checkstrengthen()
    {
        // ��ȡ��Χ�ĳ���Ͷ����
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, shootThrowThingLen, LayerMask.GetMask("ǿ����"));
        // ��Ϣ���Ĵ洢 <����Ͷ����> ��Ϣ
        EventCenter.GetInstance().EventTrigger<Collider2D[]>("ǿ����", cols);
    }
    /// <summary>
    /// Ѱ�������������һ������
    /// </summary>
    private void FindProximityOfEnemy()
    {
        // ��ȡ��Χ�ڵĵ���
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, shootEnemyLen, LayerMask.GetMask("����"));
        // �����Χ��û��
        if (cols.Length == 0)
        {
            // ��Ϣ���Ĵ洢 <��������ĵ���> ��Ϣ
            EventCenter.GetInstance().EventTrigger<Collider2D>("��������ĵ���", null);
            return;
        }
        // ��������
        GameTool.QuickSortArray(transform.position, cols, 0, cols.Length - 1);
        // ��Ϣ���Ĵ洢 <��������ĵ���> ��Ϣ
        EventCenter.GetInstance().EventTrigger<Collider2D>("��������ĵ���", cols[0]);

    }
    /// <summary>
    /// ��鸽��Npc
    /// </summary>
    private void CheckNpcHere()
    {
        Collider2D cols = Physics2D.OverlapCircle(transform.position, shootThrowThingLen, LayerMask.GetMask("Npc"));
        // ��Ϣ���Ĵ洢 <Npc> ��Ϣ
        EventCenter.GetInstance().EventTrigger<Collider2D>("������Npc", cols);
    }
    private void OnDrawGizmosSelected()
    {
        // ����Ͷ����Ȧ
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shootThrowThingLen);
        // ��������Ȧ
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, shootEnemyLen);
    }
}
