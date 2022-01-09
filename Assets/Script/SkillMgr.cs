using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMgr
{
    /// <summary>
    /// 技能--火球
    /// </summary>
    /// <param name="owner">发射点</param>
    /// <param name="dir">目标法向</param>
    /// <param name="off">偏移</param>
    /// <param name="speed">速度</param>
    /// <param name="who">谁发射的</param>
    public static void SkillOfFireBall(Vector3 owner, Vector3 dir, Vector3 off = new Vector3(),float speed = 10,WhoShoot who = WhoShoot.Enemy)
    {
        PoolMgr.GetInstance().GetObj("Prefabs/Bullet/FireBall", (x) =>
        {
            x.transform.position = owner + off;
            // 设置火球速度
            x.GetComponent<Rigidbody2D>().velocity = dir * speed;
            // 设置火球朝向
            x.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            // 设置火球发射者
            x.GetComponent<ThrowItem>().ws = who;
        });
    }
    public static void SkillOfDeathHand(Vector3 owner, int num = 10, float radius = 10f)
    {
        //                  个数
        for (int i = 0; i < num; i++)
        {
            PoolMgr.GetInstance().GetObj("Prefabs/Skills/DeathHand", (x) =>
             {
                 //                圆形区域范围随机   *  圆的范围
                 Vector3 p = Random.insideUnitCircle * radius;
                 //                         内空心圆半径 + 空心圆的范围 => 整个空心圆半径
                 Vector3 pos = p.normalized * (3 + p.magnitude);
                 x.transform.position = owner + new Vector3(pos.x, pos.y, 0);

             });
        }

    }
}
