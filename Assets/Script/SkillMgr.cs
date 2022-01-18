using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMgr
{
    /// <summary>
    /// 一个指向性
    /// </summary>
    /// <param name="id">技能id</param>
    /// <param name="owner">释放本体位置</param>
    /// <param name="dir">指向方向</param>
    /// <param name="off">技能起始位偏移</param>
    /// <param name="who">谁放的</param>
    public static void SkillOfOnePoint(int id,Vector3 owner, Vector3 dir, Vector3 off = new Vector3(), WhoShoot who = WhoShoot.Enemy)
    {
        SkillData data = Datas.GetInstance().SkillDataDic[id];
        PoolMgr.GetInstance().GetObj(data.path, (x) =>
        {
            x.transform.position = owner + off;
            // 设置火球速度
            x.GetComponent<Rigidbody2D>().velocity = dir * data.speed;
            // 设置火球朝向
            x.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            // 设置火球发射者
            x.GetComponent<ThrowItem>().ws = who;
            // 设置伤害
            x.GetComponent<ThrowItem>().hurt = data.hurt;
        });
    }
    /// <summary>
    /// 区域圆形随机
    /// </summary>
    /// <param name="id">技能id</param>
    /// <param name="owner">释放本体位置</param>
    /// <param name="num">随机数量</param>
    /// <param name="radius">最外园半径</param>
    /// <param name="InnerHollowCircle">内空心圆半径</param>
    public static void SkillOfRegionalCircularRandom(int id,Vector3 owner, int num = 10, float radius = 10f, float InnerHollowCircle = 3)
    {
        SkillData data = Datas.GetInstance().SkillDataDic[id];
        //                  个数
        for (int i = 0; i < num; i++)
        {
            PoolMgr.GetInstance().GetObj(data.path, (x) =>
             {
                 //                圆形区域范围随机   *  圆的范围
                 Vector3 p = Random.insideUnitCircle * radius;
                 //                         内空心圆半径 + 空心圆的范围 => 整个空心圆半径
                 Vector3 pos = p.normalized * (InnerHollowCircle + p.magnitude);
                 x.transform.position = owner + new Vector3(pos.x, pos.y, 0);

             });
        }

    }
}
