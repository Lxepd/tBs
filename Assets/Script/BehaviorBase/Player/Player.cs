using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BehaviorBase
{
    protected Vector2 dir;
    [SerializeField] public PlayerData playerData;
    [SerializeField] public WeaponData weaponData;
    private Cinemachine.CinemachineCollisionImpulseSource MyInpulse;
    protected Transform gun;

    private int coinNum;
    protected Collider2D nearEnemy;

    private bool isHit,isDeath;
    protected override void Start()
    {
        base.Start();

        MyInpulse = GetComponent<Cinemachine.CinemachineCollisionImpulseSource>();

        gun = GameTool.FindTheChild(gameObject, "GunSprite");
        gun.GetComponent<SpriteRenderer>().sprite = ResMgr.GetInstance().Load<Sprite>(weaponData.spritePath);
        
        EventCenter.GetInstance().EventTrigger<WeaponData>("ǹ֧����", weaponData);
        EventCenter.GetInstance().AddEventListener<int>("ǹ֧����", (x) =>
         {
             weaponData = Datas.GetInstance().WeaponDataDic[x];
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
        EventCenter.GetInstance().AddEventListener("�������", () =>
         {
             UIMgr.GetInstance().ShowPanel<EndPanel>("EndPanel", E_UI_Layer.Above);
             
             isDeath = true;
         });
        EventCenter.GetInstance().AddEventListener("�������", () =>
        {
            MyInpulse.GenerateImpulse();
            isHit = true;
        });
        EventCenter.GetInstance().AddEventListener("��ɫ�ָ�", () =>
        {
            isDeath = false;
            Time.timeScale = 1;
            gameObject.SetActive(true);
        });
    }
    protected virtual void Update()
    {
        animTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        EventCenter.GetInstance().EventTrigger<GameObject>("�������", gameObject);
        EventCenter.GetInstance().EventTrigger<Transform>("�Ƿ���ǹ֧", gun);
        EventCenter.GetInstance().EventTrigger<Vector2>("������", transform.position);
        EventCenter.GetInstance().EventTrigger<int>("��ǰ���", coinNum);
        rg.velocity = dir * playerData.speed;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Death") && animTime > 1f)
        {
            Time.timeScale = 0;
            gameObject.SetActive(false);
            return;
        }
        if (GameTool.CheckAnimatorNameAnd1f(anim,"Hurt", animTime))
        {
            return;
        }

        if (isDeath)
        {
            anim.Play("Death");
            return;
        }
        if (isHit)
        {
            isHit = false;
            anim.Play("Hurt");
            return;
        }

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

        RoleArms();
    }
    protected virtual void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener<int>("ǹ֧����", (x) =>
        {
            weaponData = Datas.GetInstance().WeaponDataDic[x];
            gun.GetComponent<SpriteRenderer>().sprite = ResMgr.GetInstance().Load<Sprite>(weaponData.spritePath);
            EventCenter.GetInstance().EventTrigger<WeaponData>("ǹ֧����", weaponData);
        });
        EventCenter.GetInstance().RemoveEventListener<Vector2>("Joystick", (x) =>
        {
            dir = x;
        });
        EventCenter.GetInstance().RemoveEventListener<int>("��ý��", (x) =>
        {
            coinNum += x;
        });
        EventCenter.GetInstance().RemoveEventListener("�������", () =>
        {
            UIMgr.GetInstance().ShowPanel<EndPanel>("EndPanel", E_UI_Layer.Above);

            isDeath = true;
        });
        EventCenter.GetInstance().RemoveEventListener("�������", () =>
        {
            isHit = true;
        });
        EventCenter.GetInstance().RemoveEventListener("��ɫ�ָ�", () =>
        {
            isDeath = false;
            Time.timeScale = 1;
            gameObject.SetActive(true);
        });
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
    protected virtual void RoleArms()
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
