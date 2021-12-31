using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public int id;

    private void Awake()
    {
        instance = this;
    }

    private Vector2 dir;
    Rigidbody2D rg;
    private Animator animator;

    [Tooltip("��������Ͷ����ķ�Χ")]
    public float shootThrowThingLen = .5f;
    [Tooltip("�����������˵ķ�Χ")]
    public float shootEnemyLen = 3f;

    public Animator Animator { get => animator; set => animator = value; }

    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // ��ȡ��Ϣ������ Joystick ����Ϣ��Ȼ��ִ�� ί��
        // ��ȡҡ�˷�������
        EventCenter.GetInstance().AddEventListener<Vector2>("Joystick", (x) => { this.dir = x; });

        EventCenter.GetInstance().EventTrigger<Animator>("Player����", animator);
    }

    // Update is called once per frame
    void Update()
    {
        EventCenter.GetInstance().EventTrigger<Vector2>("PlayerPos", transform.position);

        animator.SetFloat("RunX", dir.x);
        animator.SetFloat("RunY", dir.y);
        animator.SetFloat("Magnitude", dir.magnitude);
        if (dir.magnitude > 0)
        {
            animator.SetFloat("IdleX", animator.GetFloat("RunX"));
            animator.SetFloat("IdleY", animator.GetFloat("RunY"));
            //animator.SetFloat("AtkX", animator.GetFloat("RunX"));
            //animator.SetFloat("AtkY", animator.GetFloat("RunY"));
        }

        rg.velocity = dir * GameMgr.GetInstance().GetPlayerInfo(id).speed;
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
        if(cols.Length==0)
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
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // ����Ͷ����Ȧ
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shootThrowThingLen);
        // ��������Ȧ
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, shootEnemyLen);
    }
#endif
}
