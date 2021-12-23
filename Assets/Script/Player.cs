using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 dir;
    Rigidbody2D rg;

    public float speed=0;
    [Tooltip("��������Ͷ����ķ�Χ")]
    public float shootThrowThingLen = .5f;
    [Tooltip("�����������˵ķ�Χ")]
    public float shootEnemyLen = 3f;

    // Start is called before the first frame update
    void Start()
    {
        rg=GetComponent<Rigidbody2D>();
        // ��ȡ��Ϣ������ Joystick ����Ϣ��Ȼ��ִ�� ί��
        // ��ȡҡ�˷�������
        EventCenter.GetInstance().AddEventListener<Vector2>("Joystick", (x)=> { this.dir = x; });
    }

    // Update is called once per frame
    void Update()
    {
        EventCenter.GetInstance().EventTrigger<Vector2>("PlayerPos", transform.position);

        rg.velocity = dir*speed;

    }
    private void FixedUpdate()
    {
        CheckMissileScope();
        FindProximityOfEnemy();
        CheckNpcHere();
    }
    /// <summary>
    /// ��鸽����û�г���Ͷ����
    /// </summary>
    private void CheckMissileScope()
    {
        // ��ȡ��Χ�ĳ���Ͷ����
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, shootThrowThingLen, LayerMask.GetMask("����Ͷ����"));
        // ��Ϣ���Ĵ洢 <����Ͷ����> ��Ϣ
        EventCenter.GetInstance().EventTrigger<Collider2D[]>("����Ͷ����", cols);
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
