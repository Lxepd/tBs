using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 状态基类
/// </summary>
public class StateBase
{
    // 状态id
    public int id { get; set; }
    public StateMachine machine;
    public StateBase(int id)
    {
        this.id = id;
    }
    // 初次进入状态
    public virtual void OnEnter(params object[] args) { }
    // 状态进行
    public virtual void OnStay(params object[] args) { }
    // 退出状态
    public virtual void OnExit(params object[] args) { }
}
/// <summary>
/// 状态控制器
/// </summary>
/// <typeparam name="T">对象控制器</typeparam>
public class StateBaseTemplate<T> : StateBase
{
    // 状态拥有者
    public T owner;
    // 调用该构造函数之前先调用父类带一个参数的构造函数
    public StateBaseTemplate(int _id, T _owner) : base(_id)
    {
        owner = _owner;
    }
}
/// <summary>
/// 状态机
/// </summary>
public class StateMachine
{
    public Dictionary<int, StateBase> m_StateCache;
    // 上一个状态
    public StateBase m_prviousState;
    // 当前状态
    public StateBase m_currentState;

    public StateMachine(StateBase beginState)
    {
        m_prviousState = null;
        m_currentState = beginState;

        m_StateCache = new Dictionary<int, StateBase>();

        AddState(beginState);
        m_currentState.OnEnter();
    }
    public void AddState(StateBase state)
    {
        if(!m_StateCache.ContainsKey(state.id))
        {
            m_StateCache.Add(state.id, state);
            state.machine = this;
        }
    }
    public void TranslateState(int id, params object[] args)
    {
        if(!m_StateCache.ContainsKey(id))
        {
            return;
        }
        //if(m_currentState != null)
        //{
        //    m_currentState.OnExit();
        //}

        m_prviousState = m_currentState;
        m_currentState = m_StateCache[id];
        m_currentState.OnEnter(args);
    }
    public void Update()
    {
        if(m_currentState!=null)
        {
            m_currentState.OnStay();
        }
    }
}