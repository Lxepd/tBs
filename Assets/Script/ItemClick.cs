using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemClick : MonoBehaviour
{
    private BoxCollider2D boxCol;
    private Image img;
    private Transform selectGo;

    public int id;
    public int currentNum; // ��ǰ����

    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        img = GameTool.FindTheChild(gameObject, "Img").GetComponent<Image>();
        selectGo = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "ItemSelect");
        selectGo.gameObject.SetActive(false);

    }
    private void Update()
    {
        transform.Find("ItemNum").GetComponent<Text>().text = currentNum.ToString();

        // ��ָ���� ���� �ǵ�һ����Ļ����
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            // ��Ʒ����ײ���� Ϊ ����λ��
            if (boxCol.OverlapPoint(Input.touches[0].position))
            {
                selectGo.gameObject.SetActive(true);
                // ��ѡȡͼ���������λ��
                StartCoroutine(MoveSelect(Input.GetTouch(0).position));
                // ����
                GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "NameTest").GetComponent<Text>().text = img.sprite.name;
                // ������ҵ����Ϣ
                EventCenter.GetInstance().EventTrigger<ItemClick>("�̵���Ʒ", this);
            }
        }

        if(Input.GetMouseButtonDown(0) && boxCol.OverlapPoint(Input.mousePosition))
        {
            GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "NameTest").GetComponent<Text>().text = img.sprite.name;

            EventCenter.GetInstance().EventTrigger<ItemClick>("�̵���Ʒ", this);

        }

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
