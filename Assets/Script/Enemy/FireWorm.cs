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
    public EnemyData Data { get => data; }
    // 注册状态计时器
    public Timer moveTimer, atkTimer, HitTimer, DeadTimer;
    // 获取父类的组件信息
    public Animator Animator { get => animator; } 
    public Rigidbody2D Rg { get => rg; }
    // 移动状态的移动执行时间
    [HideInInspector] public float MoveTimertime;
    // 攻击状态的攻击执行时间
    [HideInInspector] public float AtkTimertime;
    // 受击状态的受击执行时间
    [HideInInspector] public float HitTimertime;
    // 死亡状态的消失执行时间
    [HideInInspector] public float DeadTimertime;

    public Timer ForceStopTimer { get => forceStopTimer; }
    public bool isHit;
    public bool isDead;
    public bool isPushInPool;

    FireWormState fws = FireWormState.Idle;

    protected override void Start()
    {
        id = 15001;
        base.Start();

        MoveTimertime = .5f * GameTool.GetAnimatorLength(animator, "Walk");
        moveTimer = new Timer(MoveTimertime, false, false);
        AtkTimertime = .65f * GameTool.GetAnimatorLength(animator, "Atk");
        atkTimer = new Timer(AtkTimertime, false, false);
        HitTimertime = .3f * GameTool.GetAnimatorLength(animator, "Hit");
        HitTimer = new Timer(HitTimertime, false, false);
        DeadTimertime = 3f * GameTool.GetAnimatorLength(animator, "Dead");
        DeadTimer = new Timer(DeadTimertime, false, false);

        // 状态机状态注册
        FireWormIdle idle = new FireWormIdle(1, this);
        FireWormWalk walk = new FireWormWalk(2, this);
        FireWormAtk atk = new FireWormAtk(3, this);
        FireWormHit hit = new FireWormHit(4, this);
        FireWormDead dead = new FireWormDead(5, this);

        // 设置初始状态
        machine = new StateMachine(idle);
        // 添加状态
        machine.AddState(walk);
        machine.AddState(atk);
        machine.AddState(hit);
        machine.AddState(dead);

        // 获取<敌人扣血>的消息
        EventCenter.GetInstance().AddEventListener<ThrowItemData>("敌人扣血", (x) =>
        {
            if (nearThrow == Vector2.zero)
                return;

            currentHp -= x.hurt;
            if (currentHp <= 0)
            {
                isDead = true;
                DeadTimer.Start();
                fws = FireWormState.Dead;
                return;
            }
            // 力 = 投掷物来向 * 投掷物重量
            //rg.AddForce(nearThrow * x.mass, ForceMode2D.Impulse);
            isHit = true;
        });

    }
    protected override void Update()
    {
        if (isPushInPool)
            return;

        base.Update();

        switch (fws)
        {
            case FireWormState.Idle:
                machine.TranslateState(1);
                break;
            case FireWormState.Walk:
                machine.TranslateState(2, player);
                if (moveTimer.isStop)
                {
                    moveTimer.Start();
                }
                break;
            case FireWormState.Atk:
                machine.TranslateState(3, player);
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
    /// 状态改变
    /// </summary>
    public override void CheckState()
    {
        player = Physics2D.OverlapCircle(transform.position, data.moveLen, LayerMask.GetMask("玩家"));

        if (!isDead)
        {
            if (player == null)
            {
                fws = FireWormState.Idle;
                return;
            }

            fws = FireWormState.Idle;


            if (isHit)
            {
                fws = FireWormState.Hit;
                return;
            }
            //如果在攻击范围内，则进入 攻击状态
            if (Vector2.Distance(player.transform.position, transform.position) <= data.atkLen)
            {
                fws = FireWormState.Atk;
                return;
            }
            // 否则移动追击状态
            else
            {
                fws = FireWormState.Walk;
                return;
            }
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (data == null)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, data.hitLen);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, data.moveLen);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, data.atkLen);
    }
#endif
}

    #region 状态
/// <summary>
/// 站立状态
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
        //Debug.Log("站立状态！");
    }
    public override void OnExit(params object[] args)
    {

    }
}
/// <summary>
/// 移动状态
/// </summary>
public class FireWormWalk : StateBaseTemplate<FireWorm>
{
    Collider2D player;
    public FireWormWalk(int id, FireWorm ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.Animator.Play("Walk");
        player = (Collider2D)args[0];
    }
    public override void OnStay(params object[] args)
    {
        //Debug.Log("移动状态！");

        Vector2 playerDir = (player.transform.position - owner.transform.position).normalized;

        if (owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > GameTool.GetAnimatorLen(owner.Animator, "Walk"))
        {
            OnExit();
            return;
        }

        if (owner.moveTimer.isTimeUp)
        {
            // 朝向玩家
            owner.Rotate(playerDir);
            // 刚体移动 = 朝向 * 怪物速度
            owner.Rg.velocity = playerDir * owner.Data.speed;

        }

    }
    public override void OnExit(params object[] args)
    {
        owner.moveTimer.Reset(owner.MoveTimertime);
        owner.Rg.velocity = Vector2.zero;
        machine.TranslateState(1);
    }
}
/// <summary>
/// 移动状态
/// </summary>
public class FireWormAtk : StateBaseTemplate<FireWorm>
{
    Collider2D player;
    bool isAtk;
    public FireWormAtk(int id, FireWorm ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.Animator.Play("Atk");
        owner.Rg.velocity = Vector2.zero;
        player = (Collider2D)args[0];
    }
    public override void OnStay(params object[] args)
    {
        //Debug.Log("攻击状态！");

        Vector3 yoff = new Vector3(.5f * owner.transform.localScale.x, 1f, 0);
        Vector3 playerYoff = new Vector3(0, .5f, 0);
        Vector3 playerDir = ((player.transform.position+ playerYoff) - owner.transform.position - yoff).normalized;
        
        owner.Rotate(playerDir);

        if (owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > GameTool.GetAnimatorLen(owner.Animator, "Atk"))
        {
            OnExit();
            return;
        }

        if (owner.atkTimer.isTimeUp && !isAtk)
        {
            isAtk = true;
            SkillMgr.SkillOfFireBall(owner.transform.position, playerDir, yoff);
            //PoolMgr.GetInstance().GetObj("Prefabs/Bullet/FireBall", (x) =>
            // {
            //     x.transform.position = owner.transform.position + yoff;
            //     // 设置火球速度
            //     x.GetComponent<Rigidbody2D>().velocity = playerDir * 10;
            //     // 设置火球朝向
            //     x.transform.rotation = Quaternion.FromToRotation(Vector3.right, playerDir);
            //     // 设置火球发射者
            //     x.GetComponent<ThrowItem>().ws = WhoShoot.Enemy;
            // });
        }

    }
    public override void OnExit(params object[] args)
    {
        isAtk = false;
        owner.atkTimer.Reset(owner.AtkTimertime, false);
        machine.TranslateState(1);

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
        //Debug.Log("被击状态！");

        if (owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > GameTool.GetAnimatorLen(owner.Animator, "Hit"))
        {
            OnExit();
            return;
        }

        if (owner.HitTimer.isTimeUp)
        {
            owner.Rg.velocity = Vector2.zero;
            
            Debug.Log("被打！！");
        }
    }
    public override void OnExit(params object[] args)
    {
        owner.isHit = false;
        owner.moveTimer.Reset(owner.MoveTimertime);
        owner.atkTimer.Reset(owner.AtkTimertime);
        owner.HitTimer.Reset(owner.HitTimertime);
    }
}
public class FireWormDead : StateBaseTemplate<FireWorm>
{
    bool isDisappear;
    Timer disappearTimer;

    public FireWormDead(int id, FireWorm ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        if (!isDisappear)
        {
            disappearTimer = new Timer(1.5f, false, false);
            disappearTimer.Start();
            owner.GetComponent<BoxCollider2D>().enabled = false;
            owner.Animator.Play("Dead");
            owner.Rg.velocity = Vector2.zero;
        }
    }
    public override void OnStay(params object[] args)
    {
        Debug.Log("死亡状态！");
        
        if(owner.DeadTimer.isTimeUp)
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
        owner.isPushInPool = true;
    }
}
#endregion