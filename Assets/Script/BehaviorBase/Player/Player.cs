using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BehaviorBase
{
    private Vector2 dir;
    [SerializeField] public PlayerData playerData;
    [SerializeField] public WeaponData weaponData;
    Transform gun;
    protected override void Start()
    {
        base.Start();

        gun = GameTool.FindTheChild(gameObject, "GunSprite");
        gun.GetComponent<SpriteRenderer>().sprite = ResMgr.GetInstance().Load<Sprite>(weaponData.spritePath);
        EventCenter.GetInstance().EventTrigger<WeaponData>("枪支数据", weaponData);
        EventCenter.GetInstance().AddEventListener<int>("枪支更新", (x) =>
         {
             weaponData = GameTool.GetDicInfo(Datas.GetInstance().WeaponDataDic, x);
             gun.GetComponent<SpriteRenderer>().sprite = ResMgr.GetInstance().Load<Sprite>(weaponData.spritePath);
             EventCenter.GetInstance().EventTrigger<WeaponData>("枪支数据", weaponData);
         });

        EventCenter.GetInstance().AddEventListener<Vector2>("Joystick", (x) => 
        {
            dir = x;
            Rotate(x);
        });
        EventCenter.GetInstance().EventTrigger<PlayerData>("角色初始", playerData);
    }
    private void Update()
    {
        EventCenter.GetInstance().EventTrigger<Transform>("是否有枪支", gun);
        EventCenter.GetInstance().EventTrigger<Vector2>("PlayerPos", transform.position);
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
    private void FixedUpdate()
    {
        Checkstrengthen();
        FindProximityOfEnemy();
        CheckNpcHere();

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
            EventCenter.GetInstance().EventTrigger<Collider2D>("距离最近的敌人", null);
            return;
        }
        // 进行排序
        GameTool.QuickSortArray(transform.position, cols, 0, cols.Length - 1);
        // 消息中心存储 <距离最近的敌人> 消息
        EventCenter.GetInstance().EventTrigger<Collider2D>("距离最近的敌人", cols[0]);
        
        RoleArms(cols[0].transform);
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
    protected virtual void RoleArms(Transform transform)
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
