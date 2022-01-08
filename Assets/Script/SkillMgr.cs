using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMgr
{
    /// <summary>
    /// ����--����
    /// </summary>
    /// <param name="owner">�����</param>
    /// <param name="dir">Ŀ�귨��</param>
    /// <param name="off">ƫ��</param>
    /// <param name="speed">�ٶ�</param>
    /// <param name="who">˭�����</param>
    public static void SkillOfFireBall(Vector3 owner, Vector3 dir, Vector3 off = new Vector3(),float speed = 10,WhoShoot who = WhoShoot.Enemy)
    {
        PoolMgr.GetInstance().GetObj("Prefabs/Bullet/FireBall", (x) =>
        {
            x.transform.position = owner + off;
            // ���û����ٶ�
            x.GetComponent<Rigidbody2D>().velocity = dir * speed;
            // ���û�����
            x.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            // ���û�������
            x.GetComponent<ThrowItem>().ws = who;
        });
    }
}
