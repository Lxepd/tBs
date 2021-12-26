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

    private void Start()
    {
        // TODO: 初始化怪物

        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        forceTimer = new Timer(.2f, false, false);
        atkTimer = new Timer(.75f * GameTool.GetAnimatorLength(animator, "Atk"), false, false);
        moveTimer = new Timer(.5f * GameTool.GetAnimatorLength(animator, "Walk"), false, false);

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
        if (dir.x < 0)
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
        Debug.Log("玩家不在附近，站立状态！");
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
        Vector2 playerDir = (player.transform.position - owner.transform.position).normalized;

        if(owner.moveTimer.isTimeUp)
        {
            // 朝向玩家
            owner.Rotate(playerDir);
            // 刚体移动 = 朝向 * 怪物速度
            owner.rg.velocity = playerDir * 3;

            if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > GameTool.GetAnimatorLength(owner.animator, "Walk"))
            {
                OnExit();
            }
        }

    }
    public override void OnExit(params object[] args)
    {
        owner.moveTimer.Reset(.5f * GameTool.GetAnimatorLength(owner.animator, "Walk"));
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
        Vector2 playerDir = (player.transform.position - owner.transform.position).normalized;
        
        owner.Rotate(playerDir);

        if(owner.atkTimer.isTimeUp)
        {
            Debug.Log("攻击");

        }

        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > GameTool.GetAnimatorLength(owner.animator, "Atk"))
        {
            OnExit();
        }
    }
    public override void OnExit(params object[] args)
    {
        owner.atkTimer.Reset(.75f * GameTool.GetAnimatorLength(owner.animator, "Atk"));
        machine.TranslateState(1);
    }
}