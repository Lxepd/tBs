using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// ״̬����
/// </summary>
public class StateBase
{
    // ״̬id
    public int id { get; set; }
    public StateMachine machine;
    public StateBase(int id)
    {
        this.id = id;
    }
    // ���ν���״̬
    public virtual void OnEnter(params object[] args) { }
    // ״̬����
    public virtual void OnStay(params object[] args) { }
    // �˳�״̬
    public virtual void OnExit(params object[] args) { }
}
/// <summary>
/// ״̬������
/// </summary>
/// <typeparam name="T">���������</typeparam>
public class StateBaseTemplate<T> : StateBase
{
    // ״̬ӵ����
    public T owner;
    // ���øù��캯��֮ǰ�ȵ��ø����һ�������Ĺ��캯��
    public StateBaseTemplate(int _id, T _owner) : base(_id)
    {
        owner = _owner;
    }
}
/// <summary>
/// ״̬��
/// </summary>
public class StateMachine
{
    public Dictionary<int, StateBase> m_StateCache;
    // ��һ��״̬
    public StateBase m_prviousState;
    // ��ǰ״̬
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