using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���ߵ��
/// </summary>
public class ItemClick : ImgClickBase
{
    // ����ID
    [HideInInspector] public int id;
    // ��ǰ������ߵ�����
    [HideInInspector] public int currentNum;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        img = GameTool.FindTheChild(gameObject, "Img").GetComponent<Image>();

        EventCenter.GetInstance().AddEventListener("���߼���", () =>
        {
            
        });
    }
    private void Update()
    {
        // ���µ�������
        transform.Find("ItemNum").GetComponent<Text>().text = currentNum.ToString();
        // �ƶ��˵��
        PhoneTouch(() =>
        {
            // ����
            GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "NameTest").GetComponent<Text>().text = img.sprite.name;
            // ������ҵ����Ϣ
            EventCenter.GetInstance().EventTrigger<ItemClick>("�̵���Ʒ", this);
            Datas.GetInstance().clickitem = this;
        });
        // PC�˵��
        PCMouseDown(() =>
        {
            GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "NameTest").GetComponent<Text>().text = img.sprite.name;
            EventCenter.GetInstance().EventTrigger<ItemClick>("�̵���Ʒ", this);
            Datas.GetInstance().clickitem = this;
        });

    }
}
