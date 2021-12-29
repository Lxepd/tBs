using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemClick : MonoBehaviour
{
    private BoxCollider2D boxCol;
    private Image img;

    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        img = GameTool.FindTheChild(gameObject, "Img").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // ��ָ���� ���� �ǵ�һ����Ļ����
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            // ��Ʒ����ײ���� Ϊ ����λ��
            if (boxCol.OverlapPoint(Input.touches[0].position))
            {
                GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "NameTest").GetComponent<Text>().text = img.sprite.name;
            }
        }
    }
}
