using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireWormState
{
    None,
    Idle,
    Walk,
    Atk,
    Hit,
    Dead
}
public class FireWorm : EnemyBase
{
    // �ƶ�״̬���ƶ�ִ��ʱ��
    [HideInInspector] public float MoveTimertime;
    // ����״̬�Ĺ���ִ��ʱ��
    [HideInInspector] public float AtkTimertime;
    // �ܻ�״̬���ܻ�ִ��ʱ��
    [HideInInspector] public float HitTimertime;
    // ����״̬����ʧִ��ʱ��
    [HideInInspector] public float DeadTimertime;

    public bool isDead;

    public FireWormState fws = FireWormState.Idle;
    public int GetId { get => id; }
    public int skillId = 17001;
    public Timer MonoTimer;
    // ����״̬�Ĺ���ִ��ʱ��
    [HideInInspector] public float MonoTimertime;
    [HideInInspector] public Transform mouth;

    protected override void Start()
    {
        id = 15001;
        base.Start();
        // ״̬��״̬ע��
        FireWormIdle idle = new FireWormIdle(1, this);
        FireWormWalk walk = new FireWormWalk(2, this);
        FireWormAtk atk = new FireWormAtk(3, this);
        FireWormHit hit = new FireWormHit(4, this);
        FireWormDead dead = new FireWormDead(5, this);

        // ���ó�ʼ״̬
        machine = new StateMachine(idle);
        // ����״̬
        machine.AddState(walk);
        machine.AddState(atk);
        machine.AddState(hit);
        machine.AddState(dead);

        MoveTimertime = .5f * GameTool.GetAnimatorLength(anim, "Walk");
        moveTimer = new Timer(MoveTimertime, false, true);
        AtkTimertime = .65f * GameTool.GetAnimatorLength(anim, "Atk");
        atkTimer = new Timer(AtkTimertime, false, true);
        DeadTimertime = 3f * GameTool.GetAnimatorLength(anim, "Dead");
        DeadTimer = new Timer(DeadTimertime, false, false);
        MonoTimertime = .5f;
        MonoTimer = new Timer(MonoTimertime, false, true);

        mouth = GameTool.FindTheChild(gameObject, "mouth");
    }
    protected override void Update()
    {
        base.Update();

        switch (fws)
        {
            case FireWormState.Idle:
                machine.TranslateState(1);
                break;
            case FireWormState.Walk:
                machine.TranslateState(2);
                break;
            case FireWormState.Atk:
                machine.TranslateState(3);
                break;
            case FireWormState.Hit:
                machine.TranslateState(4);
                break;
            case FireWormState.Dead:
                machine.TranslateState(5);
                break;
        }

    }
    /// <summary>
    /// ״̬�ı�
    /// </summary>
    public override void CheckState()
    {
        base.CheckState();

        if (currentHp <= 0)
        {
            isDead = true;
            DeadTimer.Start();
            currentHp = 0;
            fws = FireWormState.Dead;
            return;
        }

        if (player == null)
        {
            fws = FireWormState.Idle;
            return;
        }
        // �������
        Rotate(player.transform.position); 

        if (isHit)
        {
            fws = FireWormState.Hit;
            return;
        }
        //����ڹ�����Χ�ڣ������ ����״̬
        if (Vector2.Distance(player.transform.position, transform.position) <= data.atkLen && MonoTimer.isTimeUp && !playerIsDeath)
        {
            MonoTimer.Reset(MonoTimertime);
            atkTimer.Start();
            fws = FireWormState.Atk;
            return;
        }
        // �����ƶ�׷��״̬
        else if(Vector2.Distance(player.transform.position, transform.position) > data.atkLen && MonoTimer.isTimeUp)
        {
            MonoTimer.Reset(MonoTimertime);
            moveTimer.Start();
            fws = FireWormState.Walk;
            return;
        }
    }
}

    #region ״̬
/// <summary>
/// վ��״̬
/// </summary>
public class FireWormIdle : StateBaseTemplate<FireWorm>
{
    public FireWormIdle(int id, FireWorm ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.Animator.Play("Idle");
    }
    public override void OnStay(params object[] args)
    {
        owner.Rg.velocity = Vector2.zero;
        //Debug.Log("վ��״̬��");
    }
    public override void OnExit(params object[] args)
    {

    }
}
/// <summary>
/// �ƶ�״̬
/// </summary>
public class FireWormWalk : StateBaseTemplate<FireWorm>
{
    public FireWormWalk(int id, FireWorm ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.Animator.Play("Walk");
    }
    public override void OnStay(params object[] args)
    {
        //Debug.Log("�ƶ�״̬��");

        if (owner.AnimaTime > 1f || owner.player == null)
        {
            OnExit();
            return;
        }

        Vector2 playerDir = (owner.player.transform.position - owner.transform.position).normalized;
        
        if (owner.moveTimer.isTimeUp)
        {
            // �����ƶ� = ���� * �����ٶ�
            owner.Rg.velocity = playerDir * owner.Data.speed;
        }

    }
    public override void OnExit(params object[] args)
    {
        owner.MonoTimer.Start();
        owner.Rg.velocity = Vector2.zero;
        owner.fws = FireWormState.Idle;
    }
}
/// <summary>
/// �ƶ�״̬
/// </summary>
public class FireWormAtk : StateBaseTemplate<FireWorm>
{
    public FireWormAtk(int id, FireWorm ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.Animator.Play("Atk");
        owner.Rg.velocity = Vector2.zero;
    }
    public override void OnStay(params object[] args)
    {
        //Debug.Log("����״̬��");

        if (owner.AnimaTime > 1f || owner.player == null)
        {
            OnExit();
            return;
        }

        Vector3 playerDir = (owner.player.transform.position - owner.mouth.position).normalized;

        if (owner.atkTimer.isTimeUp)
        {
            owner.atkTimer.Reset(owner.AtkTimertime);
            SkillMgr.SkillOfOnePoint(owner.skillId, owner.mouth.position, playerDir);
        }

    }
    public override void OnExit(params object[] args)
    {
        owner.MonoTimer.Start();
        owner.fws = FireWormState.Idle;
    }
}
public class FireWormHit : StateBaseTemplate<FireWorm>
{
    public FireWormHit(int id, FireWorm ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.Animator.Play("Hit");
        owner.Rg.velocity = Vector2.zero;
        owner.MonoTimer.Pause();
    }
    public override void OnStay(params object[] args)
    {
        //Debug.Log("����״̬��");

        if (owner.AnimaTime > 1f)
        {
            OnExit();
            return;
        }
    }
    public override void OnExit(params object[] args)
    {
        owner.Hit = false;
        owner.MonoTimer.Continue();
        owner.fws = FireWormState.Idle;
    }
}
public class FireWormDead : StateBaseTemplate<FireWorm>
{
    bool isDisappear, isDropReward;
    Timer disappearTimer;

    public FireWormDead(int id, FireWorm ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        if (!isDisappear)
        {
            disappearTimer = new Timer(.8f, false, false);
            disappearTimer.Start();
            owner.GetComponent<BoxCollider2D>().enabled = false;
            owner.Animator.Play("Dead");
            owner.Rg.velocity = Vector2.zero;
        }
        if(!isDropReward)
        {
            isDropReward = true;
            PoolMgr.GetInstance().GetObj(Datas.GetInstance().RewardDataDic[Datas.GetInstance().EnemyDataDic[owner.GetId].rewardId].path, (x) =>
            {
                x.transform.position = owner.transform.position;
            });
        }
    }
    public override void OnStay(params object[] args)
    {
        //Debug.Log("����״̬��");

        if (owner.AnimaTime > 1f)
        {
            isDisappear = true;
            owner.Animator.Play("Disappear");
        }

        if(disappearTimer.isTimeUp)
        {
            OnExit();
        }
    }
    public override void OnExit(params object[] args)
    {
        PoolMgr.GetInstance().PushObj(owner.Data.path, owner.gameObject);
        owner.stopMono = true;
        EventCenter.GetInstance().EventTrigger<GameObject>("���������", owner.gameObject);
    }
}
#endregion