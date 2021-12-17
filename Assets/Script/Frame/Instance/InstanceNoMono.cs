using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式
/// </summary>
/// <typeparam name="T"></typeparam>
/// 参数约束
/// where T : struct            | T必须是一个结构类型
/// where T : class             | T必须是一个Class类型
/// where T : new()             | T必须要有一个无参构造函数
/// where T : NameOfBaseClass   | T必须继承名为NameOfBaseClass的类
/// where T : NameOfInterface   | T必须实现名为NameOfInterface的接口
public class InstanceNoMono<T> where T:new()
{
    private static T instance;

    public static T GetInstance()
    {
        if(instance==null)
        {
            instance = new T();
        }

        return instance;
    }
}
