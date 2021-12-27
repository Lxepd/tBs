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
    [Tooltip("��ҹ�������Χ")]
    public float HitLen = 2f;
    [Tooltip("�ƶ���Χ")]
    public float MoveLen = 12f;
    [Tooltip("������Χ")]
    public float AtkLen = 10f;

    public FWState fws = FWState.Idle;
    public StateMachine machine;

    public float MoveTimertime;
    public float AtkTimertime;

    private void Start()
    {
        // TODO: ��ʼ������

        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        forceTimer = new Timer(.2f, false, false);

        MoveTimertime = .5f * GameTool.GetAnimatorLength(animator, "Walk");
        moveTimer = new Timer(MoveTimertime, false, false);
        AtkTimertime = .65f * GameTool.GetAnimatorLength(animator, "Atk");
        atkTimer = new Timer(AtkTimertime, false, false);

        // ��ȡ<���˿�Ѫ>����Ϣ
        EventCenter.GetInstance().AddEventListener<ThrowItemData>("���˿�Ѫ", (x) =>
        {
            if (nearThrow == Vector2.zero)
                return;

            Debug.Log("���˿�Ѫ��    " + x.hurt);
            animator.Play("Hit");
            // �� = Ͷ�������� * Ͷ��������
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
        Collider2D player = Physics2D.OverlapCircle(transform.position, MoveLen,LayerMask.GetMask("���"));

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
/// վ��״̬
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
        Debug.Log("վ��״̬��");
    }
    public override void OnExit(params object[] args)
    {

    }
}
/// <summary>
/// �ƶ�״̬
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
        Debug.Log("�ƶ�״̬��");

        Vector2 playerDir = (player.transform.position - owner.transform.position).normalized;

        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > GameTool.GetAnimatorLength(owner.animator, "Walk"))
        {
            OnExit();
            return;
        }

        if (owner.moveTimer.isTimeUp)
        {
            // �������
            owner.Rotate(playerDir);
            // �����ƶ� = ���� * �����ٶ�
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
/// �ƶ�״̬
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
        Debug.Log("����״̬��");

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
                 // ���û����ٶ�
                 x.GetComponent<Rigidbody2D>().velocity = playerDir * 10;
                 // ���û�����
                 x.transform.rotation = Quaternion.FromToRotation(Vector3.right, playerDir);
                 // ���û�������
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