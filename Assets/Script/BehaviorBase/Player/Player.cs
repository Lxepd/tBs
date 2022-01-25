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
        
        EventCenter.GetInstance().EventTrigger<WeaponData>("枪支数据", weaponData);
        EventCenter.GetInstance().AddEventListener<int>("枪支更新", (x) =>
         {
             weaponData = Datas.GetInstance().WeaponDataDic[x];
             gun.GetComponent<SpriteRenderer>().sprite = ResMgr.GetInstance().Load<Sprite>(weaponData.spritePath);
             EventCenter.GetInstance().EventTrigger<WeaponData>("枪支数据", weaponData);
         });

        EventCenter.GetInstance().AddEventListener<Vector2>("Joystick", (x) => 
        {
            dir = x;
        });
        EventCenter.GetInstance().EventTrigger<PlayerData>("角色初始", playerData);

        EventCenter.GetInstance().AddEventListener<int>("获得金币", (x) =>
        {
            coinNum += x;
        });
        EventCenter.GetInstance().AddEventListener("玩家死亡", () =>
         {
             UIMgr.GetInstance().ShowPanel<EndPanel>("EndPanel", E_UI_Layer.Above);
             
             isDeath = true;
         });
        EventCenter.GetInstance().AddEventListener("玩家受伤", () =>
        {
            MyInpulse.GenerateImpulse();
            isHit = true;
        });
        EventCenter.GetInstance().AddEventListener("角色恢复", () =>
        {
            isDeath = false;
            Time.timeScale = 1;
            gameObject.SetActive(true);
        });
    }
    protected virtual void Update()
    {
        animTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        EventCenter.GetInstance().EventTrigger<GameObject>("玩家物体", gameObject);
        EventCenter.GetInstance().EventTrigger<Transform>("是否有枪支", gun);
        EventCenter.GetInstance().EventTrigger<Vector2>("射击起点", transform.position);
        EventCenter.GetInstance().EventTrigger<int>("当前金币", coinNum);
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
        EventCenter.GetInstance().RemoveEventListener<int>("枪支更新", (x) =>
        {
            weaponData = Datas.GetInstance().WeaponDataDic[x];
            gun.GetComponent<SpriteRenderer>().sprite = ResMgr.GetInstance().Load<Sprite>(weaponData.spritePath);
            EventCenter.GetInstance().EventTrigger<WeaponData>("枪支数据", weaponData);
        });
        EventCenter.GetInstance().RemoveEventListener<Vector2>("Joystick", (x) =>
        {
            dir = x;
        });
        EventCenter.GetInstance().RemoveEventListener<int>("获得金币", (x) =>
        {
            coinNum += x;
        });
        EventCenter.GetInstance().RemoveEventListener("玩家死亡", () =>
        {
            UIMgr.GetInstance().ShowPanel<EndPanel>("EndPanel", E_UI_Layer.Above);

            isDeath = true;
        });
        EventCenter.GetInstance().RemoveEventListener("玩家受伤", () =>
        {
            isHit = true;
        });
        EventCenter.GetInstance().RemoveEventListener("角色恢复", () =>
        {
            isDeath = false;
            Time.timeScale = 1;
            gameObject.SetActive(true);
        });
    }
    /// <summary>
    /// 检查附近有没有强化物
    /// </summary>
    private void Checkstrengthen()
    {
        // 获取范围的场景投掷物
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, playerData.checkLen, LayerMask.GetMask("强化物"));
        // 消息中心存储 <附近投掷物> 消息
        EventCenter.GetInstance().EventTrigger<Collider2D[]>("强化物", cols);
    }
    /// <summary>
    /// 寻找离自身最近的一个敌人
    /// </summary>
    private void FindProximityOfEnemy()
    {
        // 获取范围内的敌人
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, weaponData.shootLen, LayerMask.GetMask("敌人"));
        // 如果范围内没人
        if (cols.Length == 0)
        {
            // 消息中心存储 <距离最近的敌人> 消息
            EventCenter.GetInstance().EventTrigger<Collider2D>("距离最近的敌人", (nearEnemy = null));
            return;
        }
        // 进行排序
        GameTool.QuickSortArray(transform.position, cols, 0, cols.Length - 1);
        // 消息中心存储 <距离最近的敌人> 消息
        EventCenter.GetInstance().EventTrigger<Collider2D>("距离最近的敌人", (nearEnemy = cols[0]));

    }
    /// <summary>
    /// 检查附近Npc
    /// </summary>
    private void CheckNpcHere()
    {
        Collider2D cols = Physics2D.OverlapCircle(transform.position, playerData.checkLen, LayerMask.GetMask("Npc"));
        // 消息中心存储 <Npc> 消息
        EventCenter.GetInstance().EventTrigger<Collider2D>("附近的Npc", cols);
    }
    protected virtual void RoleArms()
    {

    }
    private void OnDrawGizmosSelected()
    {
        // 检索投掷物圈
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, playerData.checkLen);
        // 检索敌人圈
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, weaponData.shootLen);
    }
}
