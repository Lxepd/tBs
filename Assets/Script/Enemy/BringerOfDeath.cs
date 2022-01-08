using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BringerOfDeathState
{
    None,
    Idle,
    Walk,
    Atk,
    Hit,
    Dead,
    Skill
}
public class BringerOfDeath : EnemyBase
{
    BringerOfDeathState bodS = BringerOfDeathState.Idle;

    public bool isDead,isSkill;
    public Timer skillTimer;
    // ¹¥»÷×´Ì¬µÄ¹¥»÷Ö´ÐÐÊ±¼ä
    [HideInInspector] public float SkillTimertime;

    protected override void Start()
    {
        id = 15002;
        base.Start();
        // ×´Ì¬»ú×´Ì¬×¢²á
        BringerOfDeathIdle idle = new BringerOfDeathIdle(1, this);
        BringerOfDeathWalk walk = new BringerOfDeathWalk(2, this);
        BringerOfDeathAtk atk = new BringerOfDeathAtk(3, this);
        BringerOfDeathHit hit = new BringerOfDeathHit(4, this);
        BringerOfDeathDead dead = new BringerOfDeathDead(5, this);
        BringerOfDeathSkill skill = new BringerOfDeathSkill(6, this);

        // ÉèÖÃ³õÊ¼×´Ì¬
        machine = new StateMachine(idle);
        // Ìí¼Ó×´Ì¬
        machine.AddState(walk);
        machine.AddState(atk);
        machine.AddState(hit);
        machine.AddState(dead);
        machine.AddState(skill);

        SkillTimertime = 2f;
        skillTimer = new Timer(SkillTimertime, false, true);
    }
    protected override void Update()
    {
        base.Update();

        switch (bodS)
        {
            case BringerOfDeathState.Idle:
                machine.TranslateState(1);
                break;
            case BringerOfDeathState.Walk:
                break;
            case BringerOfDeathState.Atk:
                machine.TranslateState(3);
                break;
            case BringerOfDeathState.Hit:
                machine.TranslateState(4);
                break;
            case BringerOfDeathState.Dead:
                machine.TranslateState(5);
                break;
            case BringerOfDeathState.Skill:
                machine.TranslateState(6);
                break;
        }
    }
    public override void CheckState()
    {
        base.CheckState();

        if (player == null)
            return;
        // ³¯ÏòÍæ¼Ò
        Rotate(player.transform.position);

        if (!isDead && !isSkill)
        {
            bodS = BringerOfDeathState.Idle;

            if (currentHp <= 0)
            {
                isDead = true;
                currentHp = 0;
                bodS = BringerOfDeathState.Dead;
                return;
            }

            if (isHit)
            {
                bodS = BringerOfDeathState.Hit;
                return;
            }

            if (Vector2.Distance(player.transform.position, transform.position) <= data.atkLen && skillTimer.isTimeUp)
            {
                isSkill = true;
                int skillIndex = Random.Range(1, 101);
                if(skillIndex<30)
                {
                    bodS = BringerOfDeathState.Atk;
                }
                else
                {
                    bodS = BringerOfDeathState.Skill;
                }
                return;
            }
     
        }
    }
}
public class BringerOfDeathIdle:StateBaseTemplate<BringerOfDeath>
{
    public BringerOfDeathIdle(int id, BringerOfDeath ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.Animator.Play("Idle");
    }
    public override void OnStay(params object[] args)
    {
        
    }
    public override void OnExit(params object[] args)
    {

    }
}
public class BringerOfDeathWalk : StateBaseTemplate<BringerOfDeath>
{
    public BringerOfDeathWalk(int id, BringerOfDeath ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.Animator.Play("Walk");
    }
    public override void OnStay(params object[] args)
    {

    }
    public override void OnExit(params object[] args)
    {

    }
}
public class BringerOfDeathAtk : StateBaseTemplate<BringerOfDeath>
{
    public BringerOfDeathAtk(int id, BringerOfDeath ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.Animator.Play("Atk");
    }
    public override void OnStay(params object[] args)
    {
        if (owner.AnimaTime > GameTool.GetAnimatorLen(owner.Animator, "Atk"))
        {
            OnExit();
            return;
        }
    }
    public override void OnExit(params object[] args)
    {
        machine.TranslateState(1);
        owner.isSkill = false;
        owner.skillTimer.Start();
    }
}
public class BringerOfDeathHit : StateBaseTemplate<BringerOfDeath>
{
    public BringerOfDeathHit(int id, BringerOfDeath ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.Animator.Play("Hit");
    }
    public override void OnStay(params object[] args)
    {
        if (owner.AnimaTime > GameTool.GetAnimatorLen(owner.Animator, "Hit"))
        {
            OnExit();
            return;
        }
    }
    public override void OnExit(params object[] args)
    {
        owner.Hit = false;
    }
}
public class BringerOfDeathDead : StateBaseTemplate<BringerOfDeath>
{
    public BringerOfDeathDead(int id, BringerOfDeath ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.Animator.Play("Death");
    }
    public override void OnStay(params object[] args)
    {
        if (owner.AnimaTime > GameTool.GetAnimatorLen(owner.Animator, "Death"))
        {
            OnExit();
            return;
        }
    }
    public override void OnExit(params object[] args)
    {
        PoolMgr.GetInstance().PushObj(owner.Data.path, owner.gameObject);

    }
}
public class BringerOfDeathSkill : StateBaseTemplate<BringerOfDeath>
{
    public BringerOfDeathSkill(int id, BringerOfDeath ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.Animator.Play("Skill");
    }
    public override void OnStay(params object[] args)
    {
        if (owner.AnimaTime > GameTool.GetAnimatorLen(owner.Animator, "Skill"))
        {
            OnExit();
            return;
        }
    }
    public override void OnExit(params object[] args)
    {
        machine.TranslateState(1);
        owner.isSkill = false;
        owner.skillTimer.Start();
    }
}