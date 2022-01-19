using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : UIBase
{
    void Start()
    {
        GetControl<Image>("RoleImg").sprite = ResMgr.GetInstance().Load<Sprite>(get);

        GetControl<Image>("KillImg").gameObject.SetActive(false);
        GetControl<Image>("GunImg").gameObject.SetActive(false);
        GetControl<Text>("TimeText").gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}
