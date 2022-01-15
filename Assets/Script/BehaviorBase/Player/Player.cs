using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BehaviorBase
{
    private Vector2 dir;
    [SerializeField] public PlayerData data;
    [Tooltip("检索附近投掷物的范围")]
    public float shootThrowThingLen = .5f;
    [Tooltip("检索附近敌人的范围")]
    public float shootEnemyLen = 3f;


    protected override void Start()
    {
        base.Start();

        EventCenter.GetInstance().AddEventListener<Vector2>("Joystick", (x) => 
        {
            dir = x;
            Rotate(x);
        });
        EventCenter.GetInstance().EventTrigger<PlayerData>("角色初始", data);
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
    /// 检查附近有没有强化物
    /// </summary>
    private void Checkstrengthen()
    {
        // 获取范围的场景投掷物
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, shootThrowThingLen, LayerMask.GetMask("强化物"));
        // 消息中心存储 <附近投掷物> 消息
        EventCenter.GetInstance().EventTrigger<Collider2D[]>("强化物", cols);
    }
    /// <summary>
    /// 寻找离自身最近的一个敌人
    /// </summary>
    private void FindProximityOfEnemy()
    {
        // 获取范围内的敌人
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, shootEnemyLen, LayerMask.GetMask("敌人"));
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

    }
    /// <summary>
    /// 检查附近Npc
    /// </summary>
    private void CheckNpcHere()
    {
        Collider2D cols = Physics2D.OverlapCircle(transform.position, shootThrowThingLen, LayerMask.GetMask("Npc"));
        // 消息中心存储 <Npc> 消息
        EventCenter.GetInstance().EventTrigger<Collider2D>("附近的Npc", cols);
    }
    private void OnDrawGizmosSelected()
    {
        // 检索投掷物圈
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shootThrowThingLen);
        // 检索敌人圈
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, shootEnemyLen);
    }
}
