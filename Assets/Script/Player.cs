using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 dir;
    Rigidbody2D rg;

    public float speed=0;

    // Start is called before the first frame update
    void Start()
    {
        rg=GetComponent<Rigidbody2D>();
        // 获取消息中心中 Joystick 的消息，然后执行 委托
        EventCenter.GetInstance().AddEventListener<Vector2>("Joystick", CheckDirChange);
    }

    // Update is called once per frame
    void Update()
    {
        EventCenter.GetInstance().EventTrigger<Vector2>("PlayerPos", transform.position);

        rg.velocity = dir*speed;
        CheckMissileScope();
        FindProximityOfEnemy();
    }
    /// <summary>
    /// 获取摇杆方向向量
    /// </summary>
    /// <param name="dir"></param>
    private void CheckDirChange(Vector2 dir)
    {
        this.dir = dir;
    }
    /// <summary>
    /// 检查附近有没有场景投掷物
    /// </summary>
    private void CheckMissileScope()
    {
        // 获取范围的场景投掷物
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(2, 2), .5f,LayerMask.GetMask("投掷物"));
        // 消息中心存储 <附近投掷物> 消息
        EventCenter.GetInstance().EventTrigger<Collider2D[]>("附近投掷物", cols);
    }
    /// <summary>
    /// 寻找离自身最近的一个敌人
    /// </summary>
    private void FindProximityOfEnemy()
    {
        // 获取范围内的敌人
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 3f, LayerMask.GetMask("敌人"));
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
    
}
