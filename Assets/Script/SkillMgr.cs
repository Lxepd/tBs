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
}
