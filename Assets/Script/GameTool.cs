using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameTool
{
    /// <summary>
    /// 快速排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">待排序组</param>
    /// <param name="start">排序起点</param>
    /// <param name="end">排序终点</param>
    public static void QuickSortArray(Vector2 pos, Collider2D[] array, int start, int end)
    {
        // 如果起点 >= 终点，跳出
        if (start >= end)
            return;

        int left = start, right = end;
        float m = GetDis(array[start], pos);
        Collider2D temp = array[start];

        while (left < right)
        {
            while (left < right && GetDis(array[right], pos) >= m)
            {
                right--;
            }

            array[left] = array[right];

            while (left < right && GetDis(array[left], pos) <= m)
            {
                left++;
            }

            array[right] = array[left];
        }

        array[right] = temp;

        QuickSortArray(pos, array, start, left - 1);
        QuickSortArray(pos, array, left + 1, end);
    }

    public static float GetDis(Collider2D array, Vector2 pos)
    {
        return Vector2.Distance(array.gameObject.transform.position, pos);
    }

    //查找子物体
    public static Transform FindTheChild(GameObject goParent, string childName)
    {
        Transform searchTrans = goParent.transform.Find(childName);
        if (searchTrans == null)
        {
            foreach (Transform trans in goParent.transform)
            {
                searchTrans = FindTheChild(trans.gameObject, childName);
                if (searchTrans != null)
                {
                    return searchTrans;
                }
            }
        }
        return searchTrans;
    }

    public static string SetTime(float second)
    {
        TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(second));
        string str = "";

        if (ts.Hours > 0)
        {
            str = ts.Hours.ToString("00") + "：" + ts.Minutes.ToString("00") + "：" + ts.Seconds.ToString("00");
        }
        if (ts.Hours == 0 && ts.Minutes > 0)
        {
            str = "00：" + ts.Minutes.ToString("00") + "：" + ts.Seconds.ToString("00");
        }
        if (ts.Hours == 0 && ts.Minutes == 0)
        {
             str = "00：" + "00：" + ts.Seconds.ToString("00");
        }

        return str;
    }
    /// <summary>
    /// 获取任意字典信息
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="dic">字典</param>
    /// <param name="id">获取的id数据</param>
    /// <returns></returns>
    public static T GetDicInfo<T>(Dictionary<int, T> dic, int id)
    {
        if (dic.ContainsKey(id))
            return dic[id];

        return default(T);
    }
    public static string GetRandomEnemyPath(bool normal = true)
    {
        int id;
        do
        {
            id = 15000 + UnityEngine.Random.Range(1, Datas.GetInstance().EnemyDataDic.Count + 1);
        } while ((normal) ? GetDicInfo(Datas.GetInstance().EnemyDataDic, id).type == EnemyType.Boss : GetDicInfo(Datas.GetInstance().EnemyDataDic, id).type == EnemyType.小怪);

        return GetDicInfo(Datas.GetInstance().EnemyDataDic, id).path;
    }
    /// <summary>
    /// List深复制
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="List">The list.</param>
    /// <returns>List{``0}.</returns>
    public static List<T> Clone<T>(object List)
    {
        using (Stream objectStream = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(objectStream, List);
            objectStream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(objectStream) as List<T>;
        }
    }
    public static float GetAnimatorLength(Animator animator, string name)
    {
        float length = 0;

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name.Equals(name))
            {
                length = clip.length;
                break;
            }
        }

        return length;
    }
    public static float GetAnimatorLen(Animator anim,string animName)
    {
        return Mathf.Min(GetAnimatorLength(anim, animName), 1f);
    }

    public static bool CheckAnimatorNameAnd1f(Animator anim,string name,float animTime)
    {
        AnimatorStateInfo asinfo = anim.GetCurrentAnimatorStateInfo(0);
        return asinfo.IsName(name) && animTime < 1f;
    }
}
