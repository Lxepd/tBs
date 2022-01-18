using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMgr
{
    /// <summary>
    /// һ��ָ����
    /// </summary>
    /// <param name="id">����id</param>
    /// <param name="owner">�ͷű���λ��</param>
    /// <param name="dir">ָ����</param>
    /// <param name="off">������ʼλƫ��</param>
    /// <param name="who">˭�ŵ�</param>
    public static void SkillOfOnePoint(int id,Vector3 owner, Vector3 dir, Vector3 off = new Vector3(), WhoShoot who = WhoShoot.Enemy)
    {
        SkillData data = Datas.GetInstance().SkillDataDic[id];
        PoolMgr.GetInstance().GetObj(data.path, (x) =>
        {
            x.transform.position = owner + off;
            // ���û����ٶ�
            x.GetComponent<Rigidbody2D>().velocity = dir * data.speed;
            // ���û�����
            x.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            // ���û�������
            x.GetComponent<ThrowItem>().ws = who;
            // �����˺�
            x.GetComponent<ThrowItem>().hurt = data.hurt;
        });
    }
    /// <summary>
    /// ����Բ�����
    /// </summary>
    /// <param name="id">����id</param>
    /// <param name="owner">�ͷű���λ��</param>
    /// <param name="num">�������</param>
    /// <param name="radius">����԰�뾶</param>
    /// <param name="InnerHollowCircle">�ڿ���Բ�뾶</param>
    public static void SkillOfRegionalCircularRandom(int id,Vector3 owner, int num = 10, float radius = 10f, float InnerHollowCircle = 3)
    {
        SkillData data = Datas.GetInstance().SkillDataDic[id];
        //                  ����
        for (int i = 0; i < num; i++)
        {
            PoolMgr.GetInstance().GetObj(data.path, (x) =>
             {
                 //                Բ������Χ���   *  Բ�ķ�Χ
                 Vector3 p = Random.insideUnitCircle * radius;
                 //                         �ڿ���Բ�뾶 + ����Բ�ķ�Χ => ��������Բ�뾶
                 Vector3 pos = p.normalized * (InnerHollowCircle + p.magnitude);
                 x.transform.position = owner + new Vector3(pos.x, pos.y, 0);

             });
        }

    }
}
