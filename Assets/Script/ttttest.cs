using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ttttest : MonoBehaviour
{
    private Animator anim;
    float animTime;
    Vector3 size = new Vector3(1.7f, 1f, 1);
    public Vector3 offV3;

    Timer handTimer;

    void Start()
    {
        anim = GetComponent<Animator>();

        handTimer = new Timer(.5f*GameTool.GetAnimatorLength(anim, "DeathHand"), false, true);
    }
    private void OnEnable()
    {
        if (handTimer != null)
            handTimer.Start();
    }
    // Update is called once per frame
    void Update()
    {
        Collider2D cols = Physics2D.OverlapBox(transform.position + offV3, size, 0, 1 << LayerMask.NameToLayer("Íæ¼Ò"));

        if (handTimer.isTimeUp && cols != null)
        {
            Debug.Log(cols.name);
            EventCenter.GetInstance().EventTrigger<float>("Íæ¼Ò¿ÛÑª", 20);
            handTimer.Reset(.5f);
        }

        animTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (animTime > GameTool.GetAnimatorLen(anim, "DeathHand"))
        {
            PoolMgr.GetInstance().PushObj(name, gameObject);
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + offV3, size);
    }
#endif
}
