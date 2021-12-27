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
    // ע��״̬��ʱ��
    public Timer moveTimer, atkTimer,HitTimer;
    // ��ȡ����������Ϣ
    public Animator Animator { get => animator; } 
    public Rigidbody2D Rg { get => rg; }
    // �ƶ�״̬���ƶ�ִ��ʱ��
    [HideInInspector] public float MoveTimertime;
    // ����״̬�Ĺ���ִ��ʱ��
    [HideInInspector] public float AtkTimertime;
    // ����״̬�Ĺ���ִ��ʱ��
    [HideInInspector] public float HitTimertime;

    [Tooltip("��ҹ�������Χ")]  public float HitLen = 2f;
    [Tooltip("�ƶ���Χ")] public float MoveLen = 12f;
    [Tooltip("������Χ")] public float AtkLen = 10f;

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

        // ״̬��״̬ע��
        FireWormIdle idle = new FireWormIdle(1, this);
        FireWormWalk walk = new FireWormWalk(2, this);
        FireWormAtk atk = new FireWormAtk(3, this);
        FireWormHit hit = new FireWormHit(4, this);
        // ���ó�ʼ״̬
        machine = new StateMachine(idle);
        // ���״̬
        machine.AddState(walk);
        machine.AddState(atk);
        machine.AddState(hit);

        // ��ȡ<���˿�Ѫ>����Ϣ
        EventCenter.GetInstance().AddEventListener<ThrowItemData>("���˿�Ѫ", (x) =>
        {
            if (nearThrow == Vector2.zero)
                return;

            Debug.Log("���˿�Ѫ��    " + x.hurt);
            // �� = Ͷ�������� * Ͷ��������
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
    /// ״̬�ı�
    /// </summary>
    public override void CheckState()
    {
        player = Physics2D.OverlapCircle(transform.position, MoveLen, LayerMask.GetMask("���"));

        // �����Χ��û�����
        if (player == null)
        {
            // ���� վ��״̬
            fws = FireWormState.Idle;
            return;
        }


        if (!isHit)
        {
            //����ڹ�����Χ�ڣ������ ����״̬
            if (Vector2.Distance(player.transform.position, transform.position) <= AtkLen)
            {
                fws = FireWormState.Atk;

            }
            // �����ƶ�׷��״̬
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
        //Debug.Log("�ƶ�״̬��");

        Vector2 playerDir = (player.transform.position - owner.transform.position).normalized;

        if (owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > GameTool.GetAnimatorLength(owner.Animator, "Walk"))
        {

            OnExit();
            return;
        }

        if (owner.moveTimer.isTimeUp)
        {
            // �������
            owner.Rotate(playerDir);
            // �����ƶ� = ���� * �����ٶ�
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
/// �ƶ�״̬
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
        //Debug.Log("����״̬��");

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
        //Debug.Log("����״̬��");

        if (owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > GameTool.GetAnimatorLength(owner.Animator, "Hit"))
        {
            OnExit();
            return;
        }

        if (owner.HitTimer.isTimeUp)
        {
            owner.Rg.velocity = Vector2.zero;
            
            Debug.Log("���򣡣�");
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