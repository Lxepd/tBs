using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 继承Mono的单例
// 不需要手动拖拽 或者 通过API去加，直接GetInstance
public class InstanceMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T GetInstance()
    {
        if (instance == null)
        {
            // 生成空物体
            GameObject obj = new GameObject();
            // 设置对象名字为脚本名
            obj.name = typeof(T).ToString();
            // 单例对象过场景时不移除
            DontDestroyOnLoad(obj);

            instance = obj.AddComponent<T>();
        }

        return instance;
    }
}