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

    [Tooltip("检索附近投掷物的范围")]
    public float shootThrowThingLen = .5f;
    [Tooltip("检索附近敌人的范围")]
    public float shootEnemyLen = 3f;

    public Animator Animator { get => animator; set => animator = value; }

    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // 获取消息中心中 Joystick 的消息，然后执行 委托
        // 获取摇杆方向向量
        EventCenter.GetInstance().AddEventListener<Vector2>("Joystick", (x) => { this.dir = x; });

        EventCenter.GetInstance().EventTrigger<Animator>("Player动画", animator);
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
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // 检索投掷物圈
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shootThrowThingLen);
        // 检索敌人圈
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, shootEnemyLen);
    }
#endif
}
