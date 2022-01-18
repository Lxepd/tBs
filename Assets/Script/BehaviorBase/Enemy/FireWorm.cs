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
    // ÒÆ¶¯×´Ì¬µÄÒÆ¶¯Ö´ÐÐÊ±¼ä
    [HideInInspector] public float MoveTimertime;
    // ¹¥»÷×´Ì¬µÄ¹¥»÷Ö´ÐÐÊ±¼ä
    [HideInInspector] public float AtkTimertime;
    // ÊÜ»÷×´Ì¬µÄÊÜ»÷Ö´ÐÐÊ±¼ä
    [HideInInspector] public float HitTimertime;
    // ËÀÍö×´Ì¬µÄÏûÊ§Ö´ÐÐÊ±¼ä
    [HideInInspector] public float DeadTimertime;

    public bool isDead;

    public FireWormState fws = FireWormState.Idle;
    public int GetId { get => id; }
    public int skillId = 17001;

    protected override void Start()
    {
        id = 15001;
        base.Start();
        // ×´Ì¬»ú×´Ì¬×¢²á
        FireWormIdle idle = new FireWormIdle(1, this);
        FireWormWalk walk = new FireWormWalk(2, this);
        FireWormAtk atk = new FireWormAtk(3, this);
        FireWormHit hit = new FireWormHit(4, this);
        FireWormDead dead = new FireWormDead(5, this);

        // ÉèÖÃ³õÊ¼×´Ì¬
        machine = new StateMachine(idle);
        // Ìí¼Ó×´Ì¬
        machine.AddState(walk);
        machine.AddState(atk);
        machine.AddState(hit);
        machine.AddState(dead);

        MoveTimertime = .5f * GameTool.GetAnimatorLength(anim, "Walk");
        moveTimer = new Timer(MoveTimertime, false, false);
        AtkTimertime = .65f * GameTool.GetAnimatorLength(anim, "Atk");
        atkTimer = new Timer(AtkTimertime, false, false);
        HitTimertime = .3f * GameTool.GetAnimatorLength(anim, "Hit");
        HitTimer = new Timer(HitTimertime, false, false);
        DeadTimertime = 3f * GameTool.GetAnimatorLength(anim, "Dead");
        DeadTimer = new Timer(DeadTimertime, false, false);
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
                if (moveTimer.isStop)
                {
                    moveTimer.Start();
                }
                break;
            case FireWormState.Atk:
                machine.TranslateState(3);
                if (atkTimer.isStop)
                {
                    atkTimer.Start();
                }
                break;
            case FireWormState.Hit:
                machine.TranslateState(4);
                if (HitTimer.isStop)
                {
                    HitTimer.Start();
                }
                break;
            case FireWormState.Dead:
                machine.TranslateState(5);
                break;
        }

    }
    /// <summary>
    /// ×´Ì¬¸Ä±ä
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
        // ³¯ÏòÍæ¼Ò
        Rotate(player.transform.position); 

        if (isHit)
        {
            fws = FireWormState.Hit;
            return;
        }
        //Èç¹ûÔÚ¹¥»÷·¶Î§ÄÚ£¬Ôò½øÈë ¹¥»÷×´Ì¬
        if (Vector2.Distance(player.transform.position, transform.position) <= data.atkLen)
        {
            fws = FireWormState.Atk;
            return;
        }
        // ·ñÔòÒÆ¶¯×·»÷×´Ì¬
        else
        {
            fws = FireWormState.Walk;
            return;
        }
    }
}

    #region ×´Ì¬
/// <summary>
/// Õ¾Á¢×´Ì¬
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
        //Debug.Log("Õ¾Á¢×´Ì¬£¡");
    }
    public override void OnExit(params object[] args)
    {

    }
}
/// <summary>
/// ÒÆ¶¯×´Ì¬
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
        //Debug.Log("ÒÆ¶¯×´Ì¬£¡");

        if (owner.AnimaTime > GameTool.GetAnimatorLen(owner.Animator, "Walk") || owner.player == null)
        {
            OnExit();
            return;
        }

        Vector2 playerDir = (owner.player.transform.position - owner.transform.position).normalized;

        if (owner.moveTimer.isTimeUp)
        {
            // ¸ÕÌåÒÆ¶¯ = ³¯Ïò * ¹ÖÎïËÙ¶È
            owner.Rg.velocity = playerDir * owner.Data.speed;
        }

    }
    public override void OnExit(params object[] args)
    {
        owner.moveTimer.Reset(owner.MoveTimertime);
        owner.Rg.velocity = Vector2.zero;
        owner.fws = FireWormState.Idle;
    }
}
/// <summary>
/// ÒÆ¶¯×´Ì¬
/// </summary>
public class FireWormAtk : StateBaseTemplate<FireWorm>
{
    bool isAtk;
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
        //Debug.Log("¹¥»÷×´Ì¬£¡");

        if (owner.AnimaTime > GameTool.GetAnimatorLen(owner.Animator, "Atk"))
        {
            OnExit();
            return;
        }

        Vector3 yoff = new Vector3(.5f * owner.transform.localScale.x, 1f, 0);
        Vector3 playerYoff = new Vector3(0, .5f, 0);
        Vector3 playerDir = ((owner.player.transform.position+ playerYoff) - owner.transform.position - yoff).normalized;

        if (owner.atkTimer.isTimeUp && !isAtk)
        {
            isAtk = true;
            SkillMgr.SkillOfOnePoint(owner.skillId, owner.transform.position, playerDir, yoff);
        }

    }
    public override void OnExit(params object[] args)
    {
        isAtk = false;
        owner.atkTimer.Reset(owner.AtkTimertime, false);
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

        owner.moveTimer.Pause();
        owner.atkTimer.Pause();
    }
    public override void OnStay(params object[] args)
    {
        //Debug.Log("±»»÷×´Ì¬£¡");

        if (owner.AnimaTime > GameTool.GetAnimatorLen(owner.Animator, "Hit"))
        {
            OnExit();
            return;
        }

        if (owner.HitTimer.isTimeUp)
        {
            owner.Rg.velocity = Vector2.zero;
            
            Debug.Log("±»´ò£¡£¡");
        }
    }
    public override void OnExit(params object[] args)
    {
        owner.Hit = false;
        owner.moveTimer.Reset(owner.MoveTimertime);
        owner.atkTimer.Reset(owner.AtkTimertime);
        owner.HitTimer.Reset(owner.HitTimertime);
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

            EventCenter.GetInstance().EventTrigger<GameObject>("¹ÖÎï±í¼õÉÙ", owner.gameObject);
        }
        if(!isDropReward)
        {
            isDropReward = true;
            PoolMgr.GetInstance().GetObj(GameTool.GetDicInfo(Datas.GetInstance().RewardDataDic, Datas.GetInstance().EnemyDataDic[owner.GetId].rewardId).path, (x) =>
            {
                x.transform.position = owner.transform.position;
            });
        }
    }
    public override void OnStay(params object[] args)
    {
        //Debug.Log("ËÀÍö×´Ì¬£¡");

        if (owner.AnimaTime > GameTool.GetAnimatorLen(owner.Animator, "Dead"))
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
    }
}
#endregion