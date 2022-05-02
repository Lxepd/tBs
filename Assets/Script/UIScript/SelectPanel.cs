using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ѡ���ɫ����
/// </summary>
public class SelectPanel : UIBase
{
    // ѡ��Ľ�ɫID
    private int playerID;

    private Transform roleSelect;

    private void Start()
    {
        Debug.Log("Select Start");

        roleSelect = GameTool.FindTheChild(gameObject, "��ɫѡ��");

        foreach (var item in Datas.GetInstance().PlayerDataDic)
        {
            PoolMgr.GetInstance().GetObj("Prefabs/��ɫ/RoleCard", (x) =>
             {
                 x.transform.SetParent(roleSelect);
                 x.transform.localScale = Vector3.one;
                 GameTool.FindTheChild(x, "RoleImg").GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(Datas.GetInstance().PlayerDataDic[item.Value.id].spritePath);

                 Toggle to = x.GetComponent<Toggle>();
                 to.group = roleSelect.GetComponent<ToggleGroup>();
                 to.onValueChanged.AddListener((y) =>
                 {
                     playerID = item.Value.id;
                     GetControl<Image>("RoleImg").sprite = ResMgr.GetInstance().Load<Sprite>(Datas.GetInstance().PlayerDataDic[playerID].spritePath);
                     GetControl<Image>("RoleImg").color = new Color(1, 1, 1, 1);
                     GetControl<Image>("RoleImg").GetComponent<Animator>().Play(Datas.GetInstance().PlayerDataDic[playerID].name);
                     Debug.Log(playerID);
                 });
             });
        }

    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            // <ѡ���ɫ����>��ť
            case "Bto_SelectPlay":
                // �ų�ûѡ�����
                if (playerID == 0)
                {
                    EventCenter.GetInstance().EventTrigger<string>("Tips", "δѡ���ɫ");
                    UIMgr.GetInstance().ShowPanel<TipsPanel>("TipsPanel", E_UI_Layer.Above);
                    return;
                }
                Datas.GetInstance().RoleId = playerID;
                InitGame();
                break;
            case "Bto_Load":
                Datas.GetInstance().gameLoad = XmlSL.GetInstance().Load();
                if (Datas.GetInstance().gameLoad == GameLoadEnum.Null)
                {
                    EventCenter.GetInstance().EventTrigger<string>("Tips", "û�浵");
                    UIMgr.GetInstance().ShowPanel<TipsPanel>("TipsPanel", E_UI_Layer.Above);
                    return;
                }
                else
                {
                    Datas.GetInstance().isLoad = true;
                    InitGame();
                }
                break;
        }
    }
    /// <summary>
    /// ��ʼ����ɫ
    /// </summary>
    /// <param name="id">ѡ��Ľ�ɫID</param>
    private void InitPlayer(int id)
    {
        GameObject playGo = ResMgr.GetInstance().Load<GameObject>(Datas.GetInstance().PlayerDataDic[id].path);
        GetRoleComponent(playGo, id);
        ResMgr.GetInstance().Load<GameObject>("Prefabs/CM vcam1").transform.SetParent(playGo.transform);

        GameObject roomGo = ResMgr.GetInstance().Load<GameObject>("Prefabs/RoomPrefabs/׼����");
        playGo.transform.position = GameObject.Find("���ص�").transform.position;
        
        // ע�᷵�����������Ϣ
        EventCenter.GetInstance().AddEventListener("���ٽ�ɫ", () =>
        {
            Destroy(playGo);
        });
        // �л�����������
        DontDestroyOnLoad(playGo);


        // �첽���ɽ�ɫ  
        //ResMgr.GetInstance().LoadAsync<GameObject>(Datas.GetInstance().PlayerDataDic[id].path, (x) =>
        //{
        //    GetRoleComponent(x, id);

        //    ResMgr.GetInstance().Load<GameObject>("Prefabs/CM vcam1").transform.SetParent(x.transform);

        //    // ��ʼ��λ��
        //    ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/RoomPrefabs/׼����", (z) =>
        //        {
        //            x.transform.position = GameObject.Find("���ص�").transform.position;
        //        });

        //    // ע�᷵�����������Ϣ
        //    EventCenter.GetInstance().AddEventListener("���ٽ�ɫ", () =>
        //        {
        //            Destroy(x);
        //        });
        //    // �л�����������
        //    DontDestroyOnLoad(x);
        //});
    }
    private void InitGame()
    {
        // ����<ѡ���ɫ>����
        UIMgr.GetInstance().HidePanel("SelectPanel");
        UIMgr.GetInstance().ShowPanel<LoadingPanel>("LoadingPanel", E_UI_Layer.Load);
        // �첽�����л�����ִ���л����ί��
        SceneMgr.GetInstance().LoadSceneAsyn("Game", () =>
        {
            // ����<������>
            UIMgr.GetInstance().HidePanel("MainPanel");

            // չ��<�������ҡ��>����
            UIMgr.GetInstance().ShowPanel<JoyStickPanel>("JoyStickPanel", E_UI_Layer.Normal);
            // չ��<�Ҳ����>����
            UIMgr.GetInstance().ShowPanel<ControlPanel>("ControlPanel", E_UI_Layer.Normal);
            // չ��<��ɫ״̬>����
            UIMgr.GetInstance().ShowPanel<StatePanel>("StatePanel", E_UI_Layer.Normal);

            // ����ѡ��Ľ�ɫID���г�ʼ��
            InitPlayer(Datas.GetInstance().isLoad ? Datas.GetInstance().RoleId : playerID);
            UIMgr.GetInstance().HidePanel("LoadingPanel");
        });
    }
    public void GetRoleComponent(GameObject go,int id)
    {
        switch (id)
        {
            case 14001:
                Dinosaur type = go.GetComponent<Dinosaur>() ?? go.AddComponent<Dinosaur>();
               break;
        }

        Datas.GetInstance().playerData = Datas.GetInstance().PlayerDataDic[id];
        Datas.GetInstance().Hp = Datas.GetInstance().PlayerDataDic[id].MaxHp;
        Datas.GetInstance().weaponData = Datas.GetInstance().WeaponDataDic[Datas.GetInstance().isLoad ? Datas.GetInstance().GunId : Datas.GetInstance().GunId = Datas.GetInstance().playerData.initialWeaponId];
    }
}
