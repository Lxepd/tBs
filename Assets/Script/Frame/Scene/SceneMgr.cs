using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// �����л�������
/// 1. �����첽����
/// 2. Э��
/// 3. ί��
/// </summary>
public class SceneMgr : InstanceNoMono<SceneMgr>
{
    /// <summary>
    /// �л�������ͬ����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void LoadScene(string name,UnityAction action)
    {
        // ����ͬ������
        SceneManager.LoadScene(name);
        // ������ɣ�ִ��action
        action();
    }
    /// <summary>
    /// �л��������첽��
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void LoadSceneAsyn(string name,UnityAction action)
    {
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadSceneAsyn(name, action));
    }
    /// <summary>
    /// Э���첽���س���
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadSceneAsyn(string name,UnityAction action)
    {
        var ao = SceneManager.LoadSceneAsync(name);
        // ��ȡ�������ؽ���
        while(!ao.isDone)
        {
            // ���¼����ķַ� ���½����� �¼�
            EventCenter.GetInstance().EventTrigger<float>("����������", ao.progress);
            yield return ao.progress;
        }

        // ������ɣ�ִ��action
        action();
    }
}
