using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �̳�Mono�ĵ���
// ����Ҫ�ֶ���ק ���� ͨ��APIȥ�ӣ�ֱ��GetInstance
public class InstanceMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T GetInstance()
    {
        if (instance == null)
        {
            // ���ɿ�����
            GameObject obj = new GameObject();
            // ���ö�������Ϊ�ű���
            obj.name = typeof(T).ToString();
            // �������������ʱ���Ƴ�
            DontDestroyOnLoad(obj);

            instance = obj.AddComponent<T>();
        }

        return instance;
    }
}