using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    protected EnemyData data;
    public EnemyData Data { get => data; }
    // 获取父类的组件信息
    // 刚体组件
    [HideInInspector] protected Rigidbody2D rg;
    public Rigidbody2D Rg { get => rg; }
    // 动画组件
    [HideInInspector] protected Animator animator;
    public Animator Animator { get => animator; }
    public float AnimaTime { get => nowAnimatorTime; }
    // 有限状态机
    [HideInInspector] protected StateMachine machine;

    // 玩家子弹来向
    [HideInInspector] protected Vector2 nearThrow;

    public bool stopMono;

    protected Timer forceStopTimer;
    public Collider2D player;

    protected int id;
    protected bool isHit;
    public bool Hit { get => isHit; set => isHit = value; }

    protected Image hp;
    protected float currentHp;
    protected float nowAnimatorTime;

    // 注册状态计时器
    public Timer moveTimer, atkTimer, HitTimer, DeadTimer;

    protected virtual void Start()
    {
        data = GameTool.GetDicInfo(Datas.GetInstance().EnemyDataDic, id);

        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        hp = GameTool.FindTheChild(gameObject, "Hp").GetComponent<Image>();
        hp.fillAmount = (currentHp = data.hp) / data.hp;

        // 获取<敌人扣血>的消息
        EventCenter.GetInstance().AddEventListener<float>("敌人扣血", (x) =>
        {
            if (nearThrow == Vector2.zero)
                return;

            currentHp -= (int)x;
            isHit = true;
        });
    }
    protected virtual void Update()
    {
        if (stopMono)
            return;

        nowAnimatorTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        // 状态选择
        CheckState();
        // 检查玩家攻击
        FindThrowDir();
        // 有限状态机更新
        machine.Update();

        hp.fillAmount = Mathf.Lerp(hp.fillAmount, currentHp / data.hp, Time.deltaTime * 10f);
    }
    // 改变朝向
    public void Rotate(Vector3 dir)
    {
        Vector3 playerDir = (dir - transform.position).normalized;
        if (playerDir.x <= 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            hp.fillOrigin = (int)Image.OriginHorizontal.Right;
        }
        else
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            hp.fillOrigin = (int)Image.OriginHorizontal.Left;
        }
    }
    // 状态改变
    public virtual void CheckState()
    {
        player = Physics2D.OverlapCircle(transform.position, data.moveLen, LayerMask.GetMask("玩家"));

    }
    /// <summary>
    /// 寻找来向投掷物
    /// </summary>
    private void FindThrowDir()
    {
        LayerMask mask = LayerMask.GetMask("基础攻击") | LayerMask.GetMask("强化攻击");
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2f, mask);
        if (cols.Length == 0)
        {
            nearThrow = Vector2.zero;
            return;
        }

        GameTool.QuickSortArray(transform.position, cols, 0, cols.Length - 1);
        nearThrow = (transform.position - cols[0].transform.position).normalized;
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

