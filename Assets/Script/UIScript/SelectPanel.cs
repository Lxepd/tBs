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

    public override void ShowMe()
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
                // չ��<�б�>����
                UIMgr.GetInstance().ShowPanel<ListPanel>("ListPanel", E_UI_Layer.Above);
                // չ��<����>����
                UIMgr.GetInstance().ShowPanel<LoadingPanel>("LoadingPanel", E_UI_Layer.Above);
                // �첽�����л�����ִ���л����ί��
                SceneMgr.GetInstance().LoadSceneAsyn("Game", () =>
                {
                    // ����<������>
                    UIMgr.GetInstance().HidePanel("MainPanel");
                    // ����<����>����
                    Invoke(nameof(ttt), 1f);
                    //UIMgr.GetInstance().HidePanel("LoadingPanel");
                    // ����<����>����
                    UIMgr.GetInstance().HidePanel("BagPanel");
                    // ����<�б�>����
                    UIMgr.GetInstance().HidePanel("ListPanel");
                    // չ��<�������ҡ��>����
                    UIMgr.GetInstance().ShowPanel<JoyStickPanel>("JoyStickPanel", E_UI_Layer.Normal);
                    // չ��<�Ҳ����>����
                    UIMgr.GetInstance().ShowPanel<ControlPanel>("ControlPanel", E_UI_Layer.Normal);
                    // չ��<��ɫ״̬>����
                    UIMgr.GetInstance().ShowPanel<StatePanel>("StatePanel", E_UI_Layer.Normal);

                    // ����ѡ��Ľ�ɫID���г�ʼ��
                    InitPlayer(playerID);
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
        ResMgr.GetInstance().LoadAsync<GameObject>(GameTool.GetDicInfo(Datas.GetInstance().PlayerDataDic, playerID).path, (x) =>
          {
             // ��ʼ��λ��
             x.transform.position = GameObject.Find("���ص�").transform.position;
             // ע�᷵�����������Ϣ
             EventCenter.GetInstance().AddEventListener<GameObject>("��ҽ�ɫ", (y) =>
              {
                  Destroy(y);
              });
              EventCenter.GetInstance().EventTrigger<GameObject>("�������", x);
             // �л�����������
             DontDestroyOnLoad(x);
          });
    }

    public void ttt()
    {
        UIMgr.GetInstance().HidePanel("LoadingPanel");
    }
}
