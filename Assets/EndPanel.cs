using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : UIBase
{
    GameObject playerGo;
    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<GameObject>("�������", (x) =>{ playerGo = x; });
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "ReturnBto":
                SceneMgr.GetInstance().LoadSceneAsyn("Game", () =>
                {
                    ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/RoomPrefabs/׼����", (z) =>
                    {
                        playerGo.transform.position = GameObject.Find("���ص�").transform.position;
                    });

                    PoolMgr.GetInstance().Clear();
                    UIMgr.GetInstance().HidePanel("EndPanel");
                    EventCenter.GetInstance().EventTrigger("��ɫ�ָ�");
                });
                break;
            case "ToMainBto":
                // �лؿ�ͷ����
                // ������Ϣ����ʼ��<ѡ���ɫ>�����ѡ��id
                EventCenter.GetInstance().EventTrigger<int>("ѡ���ɫ", 0);
                // ������Ϣ��ִ��<��ҽ�ɫ>����
                EventCenter.GetInstance().EventTrigger<GameObject>("��ҽ�ɫ", playerGo);
                EventCenter.GetInstance().EventTrigger<int>("���������", 0);
                EventCenter.GetInstance().EventTrigger<bool>("�������", true);
                EventCenter.GetInstance().Clear();
                SceneMgr.GetInstance().LoadSceneAsyn("Main", () =>
                {
                    // ����<������>����Ľ���ȫ������
                    UIMgr.GetInstance().HideAllPanelBesides("MainPanel");
                    // ��ջ����
                    PoolMgr.GetInstance().Clear();
                });
                break;
        }
    }
}
