using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���ߵ��
/// </summary>
public class ItemClick : ImgClickBase
{
    // ����ѡ��
    private Transform selectGo;
    // ����ID
    [HideInInspector] public int id;
    // ��ǰ������ߵ�����
    [HideInInspector] public int currentNum;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        img = GameTool.FindTheChild(gameObject, "Img").GetComponent<Image>();
        selectGo = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "ItemSelect");
        selectGo.gameObject.SetActive(false);

    }
    private void Update()
    {
        // ���µ�������
        transform.Find("ItemNum").GetComponent<Text>().text = currentNum.ToString();
        // �ƶ��˵��
        PhoneTouch(() =>
        {
            //selectGo.gameObject.SetActive(true);
            // ��ѡȡͼ���������λ��
            //StartCoroutine(MoveSelect(Input.GetTouch(0).position));
            // ����
            GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "NameTest").GetComponent<Text>().text = img.sprite.name;
            // ������ҵ����Ϣ
            EventCenter.GetInstance().EventTrigger<ItemClick>("�̵���Ʒ", this);

        });
        // PC�˵��
        PCMouseDown(() =>
        {
            GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "NameTest").GetComponent<Text>().text = img.sprite.name;

            EventCenter.GetInstance().EventTrigger<ItemClick>("�̵���Ʒ", this);

        });

    }
    private IEnumerator MoveSelect(Vector3 end)
    {
        while (selectGo.position != end)
        {
            selectGo.position = Vector2.MoveTowards(selectGo.position, end, 10);
            Debug.Log(selectGo.position);
            yield return 0;
        }
    }
}
