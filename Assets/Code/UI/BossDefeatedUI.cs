using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeatedUI : MonoBehaviour
{
    Animator anim;
    Canvas c;
    private void Start()
    {
        c = GetComponent<Canvas>();
        anim = GetComponent<Animator>();
        c.enabled = false;
        anim.enabled = false;

        GlobalEvents.Get.Subscribe(Events.Global.Misc.BossDefeated, e =>
        {
            StartCoroutine(DelayThenPlay());
        });


    }

    private IEnumerator DelayThenPlay()
    {
        yield return new WaitForSeconds(2f);
        anim.enabled = true;
        c.enabled = true;
        anim.PlayInFixedTime("BossUI_Defeated", -1, 0);
    }
}
