using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CtrlType
{
    ���,
    ���̵�,
    ����
}
public class ControlPanel : UIBase
{
    // ǿ�����б�
    private Collider2D[] strengthen;
    // ǿ��������
    private Text numText;
    // ǿ���ﵯϻ
    // ����ȡ�� ǿ������� ����
    private Queue<BulletBag> strengthenQueue;
    // ��ȡǿ������������
    private int maxThingNum = 10;
    // ��ǰ���λ��
    private Vector3 playerPos;
    // ����ĵ���
    private GameObject nearEnemy;
    // ���˷�������
    private Vector3 enemyDir;
    // ��ʱ��
    private Timer cdTimer;
    // Ͷ���ٶ�
    public float throwSpeed = .2f;
    // �ı�ǰ��Ͷ���ٶ�
    private float lastThrow;

    CtrlType ctrlType;
    Npc npcComponent;
    protected override void Awake()
    {
        base.Awake();
        // ��ʼ������
        if (strengthenQueue == null)
        {
            strengthenQueue = new Queue<BulletBag>();
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        // ��ȡtext���
        numText = GetControl<Text>("ThrowNum");
        // ����Ϣ������ȡ��Ϣ
        EventCenter.GetInstance().AddEventListener<Collider2D[]>("ǿ����", (x) => { strengthen = x; });
        EventCenter.GetInstance().AddEventListener<Vector2>("PlayerPos", (x) =>
        {
            playerPos = x + new Vector2(0, .5f);
        });
        EventCenter.GetInstance().AddEventListener<Collider2D>("��������ĵ���", (x) =>
        {
            if (x == null)
            {
                nearEnemy = null;
                return;
            }

            nearEnemy = x.gameObject;
        });
        EventCenter.GetInstance().AddEventListener<Collider2D>("������Npc", (x) =>
        {
            // TODO:  ��<Ͷ��>�����ϵ�ͼƬ���ĳ�<����>ͼƬ

            if (x != null)
            {
                ctrlType = CtrlType.���̵�;
                npcComponent = x.GetComponent<Npc>();
            }
            else
            {
                ctrlType = CtrlType.���;
            }

        });
        // ��ʱ��
        cdTimer = new Timer(throwSpeed, true);
        lastThrow = throwSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        // ���ݶ��������޸�text���ı�
        if (strengthenQueue.Count < 1)
        {
            numText.text = "��";
        }
        else
        {
            numText.text = strengthenQueue.Count + "/" + maxThingNum;
        }
        // Ͷ���ٶ��޸�ʱ
        if (lastThrow != throwSpeed)
        {
            // ���ü�ʱ��
            cdTimer.Reset(throwSpeed, false);
            lastThrow = throwSpeed;
        }
    }
    /// <summary>
    /// ��ť����¼�ע��
    /// </summary>
    /// <param name="btnName">��ť��</param>
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "TakeBto":
                TakeThing();
                break;
            case "ThrowBto":
                SwitchThrowKeyAct();
                break;
            case "BagBto":
                OpenBag();
                break;
        }
    }
    /// <summary>
    /// ��ȡ
    /// </summary>
    private void TakeThing()
    {
        if (strengthen.Length < 1)
        {
            Debug.Log("���˸���������");
            return;
        }

        Debug.Log("��ȡǿ�����");
        // һ��ֻ��ȡһ������������
        if (strengthenQueue.Count < maxThingNum)
        {
            BulletBag bb = strengthen[0].GetComponent<BulletBag>();
            bb.InitPos();
            strengthenQueue.Enqueue(bb);
            PoolMgr.GetInstance().PushObj(strengthen[0].name, strengthen[0].gameObject);
            Debug.Log(strengthenQueue.Count);
        }

    }
    /// <summary>
    /// ���
    /// </summary>
    private void Shoot()
    {
        // �����ϻ��û�У�����л�������
        if (strengthenQueue.Count == 0)
        {
            // ��������
            ThrowItemData data = GameTool.GetDicInfo(Datas.GetInstance().ThrowItemDataDic, 10001);
            ShootBase(data.path, data.speed);
        }
        // ����ʹ�õ�ϻ�е�һ��ǿ����
        else
        {
            BulletBag first = strengthenQueue.Dequeue();

            Debug.Log("ǿ���������        " + first);
            ThrowItemData tid = GameTool.GetDicInfo(Datas.GetInstance().ThrowItemDataDic, first.id);
            ShootBase(tid.path, tid.speed);
        }
    }
    /// <summary>
    /// �򿪱���
    /// </summary>
    private void OpenBag()
    {
        Debug.Log("�򿪱�������");
        UIMgr.GetInstance().ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Above);
    }
    /// <summary>
    /// ���
    /// </summary>
    private void ShootBase(string name, float speed)
    {
        PoolMgr.GetInstance().GetObj(name, (x) =>
        {
            x.transform.position = playerPos;

            Rigidbody2D rg = x.GetComponent<Rigidbody2D>();

            // �е��˾ͳ����˷���
            if (nearEnemy != null)
            {
                enemyDir = (nearEnemy.transform.position - playerPos).normalized;
                EventCenter.GetInstance().EventTrigger<Vector2>("Joystick", Vector2.zero);

                Player.instance.Animator.SetFloat("AtkX", enemyDir.x);
                Player.instance.Animator.SetFloat("AtkY", enemyDir.y);
                // ���Żӿ�����
                Player.instance.Animator.Play("Atk");
                // �����ٶ�
                rg.velocity = speed * enemyDir;
                // ���ó���
                x.transform.rotation = Quaternion.FromToRotation(Vector3.right, enemyDir);
            }

            x.GetComponent<ThrowItem>().ws = WhoShoot.Player;
        });
    }

    private void SwitchThrowKeyAct()
    {
        switch (ctrlType)
        {
            case CtrlType.���:
                if (nearEnemy == null || !cdTimer.isTimeUp)
                {
                    Debug.Log("����û�� ���� CDû��");
                    return;
                }
                Shoot();
                break;
            case CtrlType.���̵�:
                Debug.Log("���̵�");
                //UIMgr.GetInstance().ShowPanel<ShopPanel>("ShopPanel", E_UI_Layer.Above);
                npcComponent.InitShop();
                break;
            case CtrlType.����:
                break;
            default:
                break;
        }
    }
}
