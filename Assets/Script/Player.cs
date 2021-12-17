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
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(2, 2), .5f,LayerMask.GetMask("投掷物"));
        // 消息中心存储 <附近投掷物> 消息
        EventCenter.GetInstance().EventTrigger<Collider2D[]>("附近投掷物", cols);
    }
    /// <summary>
    /// 寻找离自身最近的一个敌人
    /// </summary>
    private void FindProximityOfEnemy()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 3f, LayerMask.GetMask("敌人"));

        QuickSortArray(cols, 0, cols.Length - 1);

        Debug.Log(cols[0].name);
        // 消息中心存储 <距离最近的敌人> 消息
        EventCenter.GetInstance().EventTrigger<Collider2D>("距离最近的敌人", cols[0]);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
    /// <summary>
    /// 快速排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">待排序组</param>
    /// <param name="start">排序起点</param>
    /// <param name="end">排序终点</param>
    public void QuickSortArray(Collider2D[] array,int start, int end)
    {
        // 如果起点 >= 终点，跳出
        if (start >= end)
            return;

        int left = start, right = end;
        float m = GetDis(array[start]);
        Collider2D temp = array[start];

        while (left<right)
        {
            while (left < right && GetDis(array[right]) >= m)
            {
                right--;
            }

            array[left] = array[right];

            while (left < right && GetDis(array[left]) <= m)
            {
                left++;
            }

            array[right] = array[left];
        }

        array[right] = temp;

        QuickSortArray(array, start, left - 1);
        QuickSortArray(array, left + 1, end);
    }

    public float GetDis(Collider2D array)
    {
        return Vector2.Distance(array.gameObject.transform.position, transform.position);
    }
}
