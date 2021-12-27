using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FWState
{
    None,
    Idle,
    Walk
}
public class FireWorm : MonoBehaviour
{
    public Rigidbody2D rg;
    public Animator animator;

    private Vector2 nearThrow;
    public Vector2 playerDir;

    public Timer forceTimer, moveTimer, atkTimer;
    [Tooltip("玩家攻击来向范围")]
    public float HitLen = 2f;
    [Tooltip("移动范围")]
    public float MoveLen = 12f;
    [Tooltip("攻击范围")]
    public float AtkLen = 10f;

    public FWState fws = FWState.Idle;
    public StateMachine machine;

    public float MoveTimertime;
    public float AtkTimertime;

    private void Start()
    {
        // TODO: 初始化怪物

        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        forceTimer = new Timer(.2f, false, false);

        MoveTimertime = .5f * GameTool.GetAnimatorLength(animator, "Walk");
        moveTimer = new Timer(MoveTimertime, false, false);
        AtkTimertime = .65f * GameTool.GetAnimatorLength(animator, "Atk");
        atkTimer = new Timer(AtkTimertime, false, false);

        // 获取<敌人扣血>的消息
        EventCenter.GetInstance().AddEventListener<ThrowItemData>("敌人扣血", (x) =>
        {
            if (nearThrow == Vector2.zero)
                return;

            Debug.Log("敌人扣血：    " + x.hurt);
            animator.Play("Hit");
            // 力 = 投掷物来向 * 投掷物重量
            rg.AddForce(nearThrow * x.mass, ForceMode2D.Impulse);
            forceTimer.Start();
        });

        EnemyIdle idle = new EnemyIdle(1, this);
        EnemyWalk walk = new EnemyWalk(2, this);
        EnemyAtk atk = new EnemyAtk(3, this);
        machine = new StateMachine(idle);
        machine.AddState(walk);
        machine.AddState(atk);
    }
    private void FixedUpdate()
    {
        CheckMoveRange();

        machine.Update();
    }
    public void CheckMoveRange()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, MoveLen,LayerMask.GetMask("玩家"));

        if (player == null)
        {
            machine.TranslateState(1);
            return;
        }

        if (Vector2.Distance(player.transform.position, transform.position) <= AtkLen)
        {
            machine.TranslateState(3, player);

            if(atkTimer.isStop)
            {
                atkTimer.Start();
            }
        }
        else
        {
            machine.TranslateState(2, player);

            if (moveTimer.isStop)
            {
                moveTimer.Start();
            }
        }
    }
    public void Rotate(Vector2 dir)
    {
        if (dir.x <= 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
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
/// <summary>
/// 站立状态
/// </summary>
public class EnemyIdle : StateBaseTemplate<FireWorm>
{
    public EnemyIdle(int id, FireWorm ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.animator.Play("Idle");
    }
    public override void OnStay(params object[] args)
    {
        Debug.Log("站立状态！");
    }
    public override void OnExit(params object[] args)
    {

    }
}
/// <summary>
/// 移动状态
/// </summary>
public class EnemyWalk : StateBaseTemplate<FireWorm>
{
    Collider2D player;
    public EnemyWalk(int id, FireWorm ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.animator.Play("Walk");
        player = (Collider2D)args[0];
    }
    public override void OnStay(params object[] args)
    {
        Debug.Log("移动状态！");

        Vector2 playerDir = (player.transform.position - owner.transform.position).normalized;

        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > GameTool.GetAnimatorLength(owner.animator, "Walk"))
        {
            OnExit();
            return;
        }

        if (owner.moveTimer.isTimeUp)
        {
            // 朝向玩家
            owner.Rotate(playerDir);
            // 刚体移动 = 朝向 * 怪物速度
            owner.rg.velocity = playerDir * 3;

        }

    }
    public override void OnExit(params object[] args)
    {
        owner.moveTimer.Reset(owner.MoveTimertime);
        owner.rg.velocity = Vector2.zero;
        machine.TranslateState(1);
    }
}
/// <summary>
/// 移动状态
/// </summary>
public class EnemyAtk : StateBaseTemplate<FireWorm>
{
    Collider2D player;
    bool isAtk;
    public EnemyAtk(int id, FireWorm ec) : base(id, ec)
    {

    }
    public override void OnEnter(params object[] args)
    {
        owner.animator.Play("Atk");
        owner.rg.velocity = Vector2.zero;
        player = (Collider2D)args[0];
    }
    public override void OnStay(params object[] args)
    {
        Debug.Log("攻击状态！");

        Vector3 yoff = new Vector3(.5f * owner.transform.localScale.x, 1f, 0);
        Vector3 playerYoff = new Vector3(0, .5f, 0);
        Vector3 playerDir = ((player.transform.position+ playerYoff) - owner.transform.position - yoff).normalized;
        
        owner.Rotate(playerDir);

        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > GameTool.GetAnimatorLength(owner.animator, "Atk"))
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