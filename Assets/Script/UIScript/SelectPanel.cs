using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ѡ���ɫ����
/// </summary>
public class SelectPanel : UIBase
{
    // ѡ��Ľ�ɫID
    private int playerID;

    private void Start()
    {
        // ע���ɫID��Ϣ
        EventCenter.GetInstance().AddEventListener<int>("ѡ���ɫ", (x) =>
         {
             Debug.Log(x);
             playerID = x;
         });
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            // <ѡ���ɫ����>��ť
            case "Bto_SelectPlay":
                // �ų�ûѡ�����
                if (playerID == 0)
                    return;
                // ����<ѡ���ɫ>����
                UIMgr.GetInstance().HidePanel("SelectPanel");
                // չ��<����>����
                UIMgr.GetInstance().ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Above);
                // չ��<����>����
                UIMgr.GetInstance().ShowPanel<LoadingPanel>("LoadingPanel", E_UI_Layer.Above);
                // �첽�����л�����ִ���л����ί��
                SceneMgr.GetInstance().LoadSceneAsyn("Game", () =>
                {
                    // ����ѡ��Ľ�ɫID���г�ʼ��
                    InitPlayer(playerID);
                    // ����<������>
                    UIMgr.GetInstance().HidePanel("MainPanel");
                    // ����<����>����
                    UIMgr.GetInstance().HidePanel("LoadingPanel");
                    // չ��<�������ҡ��>����
                    UIMgr.GetInstance().ShowPanel<JoyStickPanel>("JoyStickPanel", E_UI_Layer.Normal);
                    // չ��<�Ҳ����>����
                    UIMgr.GetInstance().ShowPanel<ControlPanel>("ControlPanel", E_UI_Layer.Normal);
                    // չ��<��ɫ״̬>����
                    UIMgr.GetInstance().ShowPanel<StatePanel>("StatePanel", E_UI_Layer.Normal);
                    // ����<����>����
                    UIMgr.GetInstance().HidePanel("BagPanel");
                });
                break;
        }
    }
    /// <summary>
    /// ��ʼ����ɫ
    /// </summary>
    /// <param name="id">ѡ��Ľ�ɫID</param>
    private void InitPlayer(int id)
    {
        // �첽���ɽ�ɫ
        ResMgr.GetInstance().LoadAsync<GameObject>(GameMgr.GetInstance().GetPlayerInfo(playerID).path, (x) =>
         {
             // ��ʼ��λ��
             x.transform.position = GameObject.Find("���ص�").transform.position;
             // ע�᷵�����������Ϣ
             EventCenter.GetInstance().AddEventListener<GameObject>("��ҽ�ɫ", (y) => 
             {
                 Destroy(y);
             });
             // �л�����������
             DontDestroyOnLoad(x);
         });
    }
}
