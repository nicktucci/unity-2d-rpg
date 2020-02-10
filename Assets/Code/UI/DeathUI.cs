using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    private Canvas c;
    private CanvasGroup grp;
    private void Start()
    {
        c = GetComponent<Canvas>();
        grp = GetComponent<CanvasGroup>();

        c.enabled = false;
        GlobalEvents.Get.Subscribe(Events.Global.Misc.CheckPointReset, data => { 
            c.enabled = false;
        });

        GlobalEvents.Get.Subscribe(Events.Global.Misc.PlayerDeath, data =>
        {
            c.enabled = true;
            grp.alpha = 0;
            GetComponent<Animator>().Play("OnDeath");
        });
    }

}
