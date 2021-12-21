using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : UIBase
{
    // Ͷ�����б�
    private Collider2D[] missiles;
    // Ͷ��������
    private Text numText;
    // Ͷ���ﵯϻ
    // ����ȡ�� Ͷ������� ����
    private Queue<string> throwMagazine;
    // ��ȡͶ������������
    private int maxThingNum = 10;

    // ��ǰ���λ��
    private Vector3 playerPos;
    // ����ĵ���
    private GameObject nearEnemy;
    // ���˷�������
    private Vector3 enemyDir;

    Timer cdTimer;
    public float throwSpeed = .5f;
    private float lastThrow;

    protected override void Awake()
    {
        base.Awake();

        if (throwMagazine == null)
        {
            throwMagazine = new Queue<string>();
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        numText = GetControl<Text>("ThrowNum");
        // ����Ϣ������ȡ��Ϣ
        EventCenter.GetInstance().AddEventListener<Collider2D[]>("����Ͷ����", (x) => { missiles = x; });
        EventCenter.GetInstance().AddEventListener<Vector2>("PlayerPos", (x) => { playerPos = x; });
        EventCenter.GetInstance().AddEventListener<Collider2D>("��������ĵ���", (x) =>
        {
            if (x == null)
            {
                nearEnemy = null;
                return;
            }

            nearEnemy = x.gameObject;
        });

        cdTimer = new Timer(throwSpeed, true);
        lastThrow = throwSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(lastThrow != throwSpeed)
        {
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
                if (nearEnemy == null || !cdTimer.isTimeUp)
                {
                    Debug.Log("����û�� ���� CDû��");
                    return;
                }

                ThrowThing();
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
        if(missiles.Length<1)
        {
            Debug.Log("���˸���������");
            return;
        }

        Debug.Log("��ȡ  Ͷ�����");
        // һ��ֻ��ȡһ������������
        if (throwMagazine.Count < maxThingNum)
        {
            throwMagazine.Enqueue(missiles[0].name);
            PoolMgr.GetInstance().PushObj(missiles[0].name, missiles[0].gameObject);
            Debug.Log(throwMagazine.Count);
        }

    }
    /// <summary>
    /// Ͷ��
    /// </summary>
    private void ThrowThing()
    {
        // �����ϻ��û�У���Ͷ��ʯͷ���й���
        if (throwMagazine.Count == 0)
        {
            Debug.Log("û�и������Ķ���������ֻ��Ͷ��ʯ����");
            ThrowBase("Prefabs/ʯ��");
        }
        // �����ӵ�ϻ�е�һ��Ͷ����
        else
        {
            string firstThrow = throwMagazine.Dequeue();
            Debug.Log(throwMagazine.Count);

            Debug.Log("����  Ͷ�����        " + firstThrow);
            ThrowBase(firstThrow);
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
    /// Ͷ��
    /// </summary>
    private void ThrowBase(string name)
    {
        Debug.Log(name);
        PoolMgr.GetInstance().GetObj(name, (x) =>
        {
            x.transform.position = playerPos;

            Rigidbody2D rg = x.GetComponent<Rigidbody2D>();

            // �е��˾ͳ����˷���
            if(nearEnemy != null)
            {
                enemyDir = (nearEnemy.transform.position - playerPos).normalized;
                rg.velocity = 5 * enemyDir;
            }

            if (x.GetComponent<ThrowItem>() == null)
            {
                x.AddComponent<ThrowItem>();
            }

        });
    }
}
