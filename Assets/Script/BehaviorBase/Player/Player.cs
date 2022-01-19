using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BehaviorBase
{
    protected Vector2 dir;
    [SerializeField] public PlayerData playerData;
    [SerializeField] public WeaponData weaponData;
    Transform gun;

    private int coinNum;
    protected Collider2D nearEnemy;

    protected override void Start()
    {
        base.Start();

        gun = GameTool.FindTheChild(gameObject, "GunSprite");
        gun.GetComponent<SpriteRenderer>().sprite = ResMgr.GetInstance().Load<Sprite>(weaponData.spritePath);
        EventCenter.GetInstance().EventTrigger<WeaponData>("ǹ֧����", weaponData);
        EventCenter.GetInstance().AddEventListener<int>("ǹ֧����", (x) =>
         {
             weaponData = GameTool.GetDicInfo(Datas.GetInstance().WeaponDataDic, x);
             gun.GetComponent<SpriteRenderer>().sprite = ResMgr.GetInstance().Load<Sprite>(weaponData.spritePath);
             EventCenter.GetInstance().EventTrigger<WeaponData>("ǹ֧����", weaponData);
         });

        EventCenter.GetInstance().AddEventListener<Vector2>("Joystick", (x) => 
        {
            dir = x;
        });
        EventCenter.GetInstance().EventTrigger<PlayerData>("��ɫ��ʼ", playerData);
        EventCenter.GetInstance().AddEventListener<int>("��ý��", (x) =>
        {
            coinNum += x;
        });
        EventCenter.GetInstance().AddEventListener<bool>("�������", (x) =>
         {
             if(x)
             {
                 Time.timeScale = 0;
                 Debug.Log("�������");
             }
         });
    }
    protected virtual void Update()
    {
        EventCenter.GetInstance().EventTrigger<Transform>("�Ƿ���ǹ֧", gun);
        EventCenter.GetInstance().EventTrigger<Vector2>("������", transform.position);
        EventCenter.GetInstance().EventTrigger<int>("��ǰ���", coinNum);
        rg.velocity = dir * playerData.speed;

        if (rg.velocity ==Vector2.zero)
        {
            anim.Play("Idle");
        }
        else
        {
            anim.Play("Run");
        }
    }
    protected virtual void FixedUpdate()
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
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, playerData.checkLen, LayerMask.GetMask("ǿ����"));
        // ��Ϣ���Ĵ洢 <����Ͷ����> ��Ϣ
        EventCenter.GetInstance().EventTrigger<Collider2D[]>("ǿ����", cols);
    }
    /// <summary>
    /// Ѱ�������������һ������
    /// </summary>
    private void FindProximityOfEnemy()
    {
        // ��ȡ��Χ�ڵĵ���
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, weaponData.shootLen, LayerMask.GetMask("����"));
        // �����Χ��û��
        if (cols.Length == 0)
        {
            // ��Ϣ���Ĵ洢 <��������ĵ���> ��Ϣ
            EventCenter.GetInstance().EventTrigger<Collider2D>("��������ĵ���", (nearEnemy = null));
            return;
        }
        // ��������
        GameTool.QuickSortArray(transform.position, cols, 0, cols.Length - 1);
        // ��Ϣ���Ĵ洢 <��������ĵ���> ��Ϣ
        EventCenter.GetInstance().EventTrigger<Collider2D>("��������ĵ���", (nearEnemy = cols[0]));
        
        RoleArms(cols[0].transform);
    }
    /// <summary>
    /// ��鸽��Npc
    /// </summary>
    private void CheckNpcHere()
    {
        Collider2D cols = Physics2D.OverlapCircle(transform.position, playerData.checkLen, LayerMask.GetMask("Npc"));
        // ��Ϣ���Ĵ洢 <Npc> ��Ϣ
        EventCenter.GetInstance().EventTrigger<Collider2D>("������Npc", cols);
    }
    protected virtual void RoleArms(Transform transform)
    {

    }
    private void OnDrawGizmosSelected()
    {
        // ����Ͷ����Ȧ
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, playerData.checkLen);
        // ��������Ȧ
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, weaponData.shootLen);
    }
}
