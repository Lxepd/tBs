using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public static float GetAnimatorLength(Animator animator, string name)
    {
        float length = 0;

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if(clip.name.Equals(name))
            {
                length = clip.length;
                break;
            }
        }

        return length;
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
}
