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

    // Start is called before the first frame update
    void Start()
    {
        numText = GetControl<Text>("ThrowNum");
        EventCenter.GetInstance().AddEventListener<Collider2D[]>("Missile", (x) => { missiles = x; });
    }

    // Update is called once per frame
    void Update()
    {

    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "TakeBto":
                TakeThing();
                break;
            case "ThrowBto":
                ThrowThing();
                break;
            case "BagBto":
                Test();
                break;
        }
    }
    private void TakeThing()
    {
        Debug.Log("��ȡ  Ͷ�����");
        // �������� ��ҽ�ɫ������ collider��
        foreach (var item in missiles)
        {
            // �ų����
            if (item.CompareTag("Ͷ����"))
            {
                Debug.Log(item.name);
            }
        }
    }
    private void ThrowThing()
    {
        Debug.Log("����  Ͷ�����");
    }

    private void Test()
    {
        Debug.Log("�򿪱�������");
        UIMgr.GetInstance().ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Above);
    }
}
