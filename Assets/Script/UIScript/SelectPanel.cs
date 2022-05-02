using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 选择角色界面
/// </summary>
public class SelectPanel : UIBase
{
    // 选择的角色ID
    private int playerID;

    private Transform roleSelect;

    private void Start()
    {
        Debug.Log("Select Start");

        roleSelect = GameTool.FindTheChild(gameObject, "角色选择");

        foreach (var item in Datas.GetInstance().PlayerDataDic)
        {
            PoolMgr.GetInstance().GetObj("Prefabs/角色/RoleCard", (x) =>
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
            // <选择角色游玩>按钮
            case "Bto_SelectPlay":
                // 排除没选择情况
                if (playerID == 0)
                {
                    EventCenter.GetInstance().EventTrigger<string>("Tips", "未选择角色");
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
                    EventCenter.GetInstance().EventTrigger<string>("Tips", "没存档");
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
    /// 初始化角色
    /// </summary>
    /// <param name="id">选择的角色ID</param>
    private void InitPlayer(int id)
    {
        GameObject playGo = ResMgr.GetInstance().Load<GameObject>(Datas.GetInstance().PlayerDataDic[id].path);
        GetRoleComponent(playGo, id);
        ResMgr.GetInstance().Load<GameObject>("Prefabs/CM vcam1").transform.SetParent(playGo.transform);

        GameObject roomGo = ResMgr.GetInstance().Load<GameObject>("Prefabs/RoomPrefabs/准备房");
        playGo.transform.position = GameObject.Find("返回点").transform.position;
        
        // 注册返回主界面的消息
        EventCenter.GetInstance().AddEventListener("销毁角色", () =>
        {
            Destroy(playGo);
        });
        // 切换场景不销毁
        DontDestroyOnLoad(playGo);


        // 异步生成角色  
        //ResMgr.GetInstance().LoadAsync<GameObject>(Datas.GetInstance().PlayerDataDic[id].path, (x) =>
        //{
        //    GetRoleComponent(x, id);

        //    ResMgr.GetInstance().Load<GameObject>("Prefabs/CM vcam1").transform.SetParent(x.transform);

        //    // 初始化位置
        //    ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/RoomPrefabs/准备房", (z) =>
        //        {
        //            x.transform.position = GameObject.Find("返回点").transform.position;
        //        });

        //    // 注册返回主界面的消息
        //    EventCenter.GetInstance().AddEventListener("销毁角色", () =>
        //        {
        //            Destroy(x);
        //        });
        //    // 切换场景不销毁
        //    DontDestroyOnLoad(x);
        //});
    }
    private void InitGame()
    {
        // 隐藏<选择角色>界面
        UIMgr.GetInstance().HidePanel("SelectPanel");
        UIMgr.GetInstance().ShowPanel<LoadingPanel>("LoadingPanel", E_UI_Layer.Load);
        // 异步场景切换，并执行切换后的委托
        SceneMgr.GetInstance().LoadSceneAsyn("Game", () =>
        {
            // 隐藏<主界面>
            UIMgr.GetInstance().HidePanel("MainPanel");

            // 展开<左侧虚拟摇杆>界面
            UIMgr.GetInstance().ShowPanel<JoyStickPanel>("JoyStickPanel", E_UI_Layer.Normal);
            // 展开<右侧控制>界面
            UIMgr.GetInstance().ShowPanel<ControlPanel>("ControlPanel", E_UI_Layer.Normal);
            // 展开<角色状态>界面
            UIMgr.GetInstance().ShowPanel<StatePanel>("StatePanel", E_UI_Layer.Normal);

            // 根据选择的角色ID进行初始化
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
