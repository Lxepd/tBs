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
    public int currentNum; // 当前数量

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

        // 单指触摸 并且 是第一次屏幕触摸
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            // 物品的碰撞体检测 为 触摸位置
            if (boxCol.OverlapPoint(Input.touches[0].position))
            {
                selectGo.gameObject.SetActive(true);
                // 将选取图标移至点击位置
                StartCoroutine(MoveSelect(Input.GetTouch(0).position));
                // 测试
                GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "NameTest").GetComponent<Text>().text = img.sprite.name;
                // 发送玩家点击消息
                EventCenter.GetInstance().EventTrigger<ItemClick>("商店物品", this);
            }
        }

        if(Input.GetMouseButtonDown(0) && boxCol.OverlapPoint(Input.mousePosition))
        {
            GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "NameTest").GetComponent<Text>().text = img.sprite.name;

            EventCenter.GetInstance().EventTrigger<ItemClick>("商店物品", this);

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
