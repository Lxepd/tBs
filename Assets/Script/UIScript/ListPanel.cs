using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �б����
/// </summary>
public class ListPanel : UIBase
{
    GameObject playerGo;
    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<GameObject>("�������", (x) => { playerGo = x; });
    }

    public override void ShowMe()
    {
        // ��ͣ��Ϸ
        Time.timeScale = 0f;
    }
    public override void HideMe()
    {
        // �ָ���Ϸ
        Time.timeScale = 1f;
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            // ��������
            case "Bto_SetMusic":
                break;
            // ���ʰ�ť
            case "Bto_Q":
                // ������UI
                UIMgr.GetInstance().ShowPanel<QuestionPanel>("QuestionPanel", E_UI_Layer.Above);
                break;
            // �������˵�
            case "Bto_Home":
                // �лؿ�ͷ����
                // ������Ϣ����ʼ��<ѡ���ɫ>�����ѡ��id
                EventCenter.GetInstance().EventTrigger<int>("ѡ���ɫ", 0);
                // ������Ϣ��ִ��<��ҽ�ɫ>����
                EventCenter.GetInstance().EventTrigger<GameObject>("��ҽ�ɫ", playerGo);
                EventCenter.GetInstance().EventTrigger<int>("���������", 0);
                EventCenter.GetInstance().EventTrigger<bool>("�������", true);
                SceneMgr.GetInstance().LoadSceneAsyn("Main", ()=>
                {
                    // ����<������>����Ľ���ȫ������
                    UIMgr.GetInstance().HideAllPanelBesides("MainPanel");
                    // ��ջ����
                    PoolMgr.GetInstance().Clear();
                });
                break;
            // �رղ˵�
            case "Bto_ListQuit":
                // ����<�б�˵�>����
                UIMgr.GetInstance().HidePanel("ListPanel");
                break;
        }
    }
}
