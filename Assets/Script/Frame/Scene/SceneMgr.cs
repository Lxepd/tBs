using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换控制器
/// 1. 场景异步加载
/// 2. 协程
/// 3. 委托
/// </summary>
public class SceneMgr : InstanceNoMono<SceneMgr>
{
    /// <summary>
    /// 切换场景（同步）
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void LoadScene(string name,UnityAction action)
    {
        // 场景同步加载
        SceneManager.LoadScene(name);
        // 加载完成，执行action
        action();
    }
    /// <summary>
    /// 切换场景（异步）
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void LoadSceneAsyn(string name,UnityAction action)
    {
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadSceneAsyn(name, action));
    }
    /// <summary>
    /// 协程异步加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadSceneAsyn(string name,UnityAction action)
    {
        var ao = SceneManager.LoadSceneAsync(name);
        // 获取场景加载进度
        while(!ao.isDone)
        {
            // 用事件中心分发 更新进度条 事件
            EventCenter.GetInstance().EventTrigger<float>("进度条更新", ao.progress);
            yield return ao.progress;
        }

        // 加载完成，执行action
        action();
    }
}
