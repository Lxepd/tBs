using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 道具点击
/// </summary>
public class ItemClick : ImgClickBase
{
    // 道具选中
    private Transform selectGo;
    // 道具ID
    [HideInInspector] public int id;
    // 当前这个道具的数量
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
        // 更新道具数量
        transform.Find("ItemNum").GetComponent<Text>().text = currentNum.ToString();
        // 移动端点击
        PhoneTouch(() =>
        {
            //selectGo.gameObject.SetActive(true);
            // 将选取图标移至点击位置
            //StartCoroutine(MoveSelect(Input.GetTouch(0).position));
            // 测试
            GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "NameTest").GetComponent<Text>().text = img.sprite.name;
            // 发送玩家点击消息
            EventCenter.GetInstance().EventTrigger<ItemClick>("商店物品", this);

        });
        // PC端点击
        PCMouseDown(() =>
        {
            GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "NameTest").GetComponent<Text>().text = img.sprite.name;

            EventCenter.GetInstance().EventTrigger<ItemClick>("商店物品", this);

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
