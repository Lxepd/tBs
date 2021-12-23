using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 dir;
    Rigidbody2D rg;

    public float speed=0;
    [Tooltip("检索附近投掷物的范围")]
    public float shootThrowThingLen = .5f;
    [Tooltip("检索附近敌人的范围")]
    public float shootEnemyLen = 3f;

    // Start is called before the first frame update
    void Start()
    {
        rg=GetComponent<Rigidbody2D>();
        // 获取消息中心中 Joystick 的消息，然后执行 委托
        // 获取摇杆方向向量
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
    /// 检查附近有没有场景投掷物
    /// </summary>
    private void CheckMissileScope()
    {
        // 获取范围的场景投掷物
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, shootThrowThingLen, LayerMask.GetMask("场景投掷物"));
        // 消息中心存储 <附近投掷物> 消息
        EventCenter.GetInstance().EventTrigger<Collider2D[]>("附近投掷物", cols);
    }
    /// <summary>
    /// 寻找离自身最近的一个敌人
    /// </summary>
    private void FindProximityOfEnemy()
    {
        // 获取范围内的敌人
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, shootEnemyLen, LayerMask.GetMask("敌人"));
        // 如果范围内没人
        if(cols.Length==0)
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
