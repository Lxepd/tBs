using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ģʽ
/// </summary>
/// <typeparam name="T"></typeparam>
/// ����Լ��
/// where T : struct            | T������һ���ṹ����
/// where T : class             | T������һ��Class����
/// where T : new()             | T����Ҫ��һ���޲ι��캯��
/// where T : NameOfBaseClass   | T����̳���ΪNameOfBaseClass����
/// where T : NameOfInterface   | T����ʵ����ΪNameOfInterface�Ľӿ�
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
