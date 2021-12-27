using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireWormState
{
    None,
    Idle,
    Walk,
    Atk,
    Hit
}
public class FireWorm : EnemyBase
{
    // 注册状态计时器
    public Timer moveTimer, atkTimer,HitTimer;
    // 获取父类的组件信息
    public Animator Animator { get => animator; } 
    public Rigidbody2D Rg { get => rg; }
    // 移动状态的移动执行时间
    [HideInInspector] public float MoveTimertime;
    // 攻击状态的攻击执行时间
    [HideInInspector] public float AtkTimertime;
    // 攻击状态的攻击执行时间
    [HideInInspector] public float HitTimertime;

    [Tooltip("玩家攻击来向范围")]  public float HitLen = 2f;
    [Tooltip("移动范围")] public float MoveLen = 12f;
    [Tooltip("攻击范围")] public float AtkLen = 10f;

    public Timer ForceStopTimer { get => forceStopTimer; }
    public bool isHit;

    FireWormState fws = FireWormState.Idle;

    protected override void Start()
    {
        base.Start();

        MoveTimertime = .5f * GameTool.GetAnimatorLength(animator, "Walk");
        moveTimer = new Timer(MoveTimertime, false, false);
        AtkTimertime = .65f * GameTool.GetAnimatorLength(animator, "Atk");
        atkTimer = new Timer(AtkTimertime, false, false);
        HitTimertime = .3f * GameTool.GetAnimatorLength(animator, "Hit");
        HitTimer = new Timer(HitTimertime, false, false);

        // 状态机状态注册
        FireWormIdle idle = new FireWormIdle(1, this);
        FireWormWalk walk = new FireWormWalk(2, this);
        FireWormAtk atk = new FireWormAtk(3, this);
        FireWormHit hit = new FireWormHit(4, this);
        // 设置初始状态
        machine = new StateMachine(idle);
        // 添加状态
        machine.AddState(walk);
        machine.AddState(atk);
        machine.AddState(hit);

        // 获取<敌人扣血>的消息
        EventCenter.GetInstance().AddEventListener<ThrowItemData>("敌人扣血", (x) =>
        {
            if (nearThrow == Vector2.zero)
                return;

            Debug.Log("敌人扣血：    " + x.hurt);
            // 力 = 投掷物来向 * 投掷物重量
            //rg.AddForce(nearThrow * x.mass, ForceMode2D.Impulse);

            isHit = true;
        });

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
        }

    }
    /// <summary>
    /// 状态改变
    /// </summary>
    public override void CheckState()
    {
        player = Physics2D.OverlapCircle(transform.position, MoveLen, LayerMask.GetMask("玩家"));

        // 如果范围内没有玩家
        if (player == null)
        {
            // 进入 站立状态
            fws = FireWormState.Idle;
            return;
        }


        if (!isHit)
        {
            //如果在攻击范围内，则进入 攻击状态
            if (Vector2.Distance(player.transform.position, transform.position) <= AtkLen)
            {
                fws = FireWormState.Atk;

            }
            // 否则移动追击状态
            else
            {
                fws = FireWormState.Walk;
            }
        }
        else
        {
            fws = FireWormState.Hit;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, HitLen);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, MoveLen);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, AtkLen);
    }
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

        if (owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > GameTool.GetAnimatorLength(owner.Animator, "Walk"))
        {

            OnExit();
            return;
        }

        if (owner.moveTimer.isTimeUp)
        {
            // 朝向玩家
            owner.Rotate(playerDir);
            // 刚体移动 = 朝向 * 怪物速度
            owner.Rg.velocity = playerDir * 3;

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

        if (owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > GameTool.GetAnimatorLength(owner.Animator, "Atk"))
        {
            
            OnExit();
            return;
        }

        if (owner.atkTimer.isTimeUp && !isAtk)
        {
            isAtk = true;
            PoolMgr.GetInstance().GetObj("Prefabs/Bullet/FireBall", (x) =>
             {
                 x.transform.position = owner.transform.position + yoff;
                 // 设置火球速度
                 x.GetComponent<Rigidbody2D>().velocity = playerDir * 10;
                 // 设置火球朝向
                 x.transform.rotation = Quaternion.FromToRotation(Vector3.right, playerDir);
                 // 设置火球发射者
                 x.GetComponent<ThrowItem>().ws = WhoShoot.Enemy;
             });
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

        if (owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > GameTool.GetAnimatorLength(owner.Animator, "Hit"))
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
#endregion