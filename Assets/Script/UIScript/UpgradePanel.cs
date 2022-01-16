using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : UIBase
{
    Transform parent;
    WeaponData weaponData;
    // Start is called before the first frame update
    void Start()
    {
        parent = GameTool.FindTheChild(gameObject, "Content");

        EventCenter.GetInstance().AddEventListener<WeaponData>("ǹ֧����", (x) => { weaponData = x; });
    }

    public override void ShowMe()
    {
        UpdateUpgradeNpc();
    }
    private void UpdateUpgradeNpc()
    {
        if (weaponData == null)
            return;

        foreach (var item in Datas.GetInstance().UpgradeDataDic)
        {
            if(item.Value.beforeId == weaponData.id)
            {
                ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/����ѡ��", (x) =>
                 {
                     x.transform.SetParent(parent);
                     x.transform.localScale = Vector3.one;

                     Image before = GameTool.FindTheChild(x, "ԭ����Img").GetComponent<Image>();
                     Image after = GameTool.FindTheChild(x, "����������Img").GetComponent<Image>();

                     before.sprite = ResMgr.GetInstance().Load<Sprite>(GameTool.GetDicInfo(Datas.GetInstance().WeaponDataDic, item.Value.beforeId).spritePath);
                     after.sprite = ResMgr.GetInstance().Load<Sprite>(GameTool.GetDicInfo(Datas.GetInstance().WeaponDataDic, item.Value.afterId).spritePath);

                 });
            }
        }
    }
}
