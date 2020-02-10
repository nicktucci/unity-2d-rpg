using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BossUI : MonoBehaviour
{
    [SerializeField]
    private ResourceBar hpBar = default;
    [SerializeField]
    private TMP_Text txtName = default;
    [SerializeField]
    private TMP_Text txtDamageTaken = default;
    private CanvasGroup grp;
    private Canvas c;


    private Unit linkRef;
    private void Start()
    {
        c = GetComponent<Canvas>();
        grp = GetComponent<CanvasGroup>();
        
        ResetState();
        GlobalEvents.Get.Subscribe(Events.Global.Misc.CheckPointReset, data =>
        {
            ResetState();
        });
        GlobalEvents.Get.Subscribe(Events.Global.Misc.TriggerBossArea, data =>
        {
            FindAndLinkBoss();
        });
        GlobalEvents.Get.Subscribe(Events.Global.Misc.PlayerDeath, data =>
        {
            if(grp.alpha == 1)
            {
                StartCoroutine(FadeOut(.4f, true));
            }
        });
    }

    private void ResetState()
    {
        StopAllCoroutines();
        c.enabled = false;
        grp.alpha = 1;
        if (linkRef != null)
        {
            hpBar.Unlink(linkRef.healthPoints);
            linkRef.healthPoints.OnChanged -= ChangeListener;
            linkRef = null;
        }
    }

    private void FindAndLinkBoss()
    {
        ResetState();

        linkRef = GameObject.FindGameObjectWithTag("Boss")?.GetComponent<Unit>();
        if (linkRef != null)
        {
            hpBar.Link(linkRef.healthPoints);
            txtName.text = linkRef.data.name;
            txtDamageTaken.text = "";
            c.enabled = true;


            linkRef.healthPoints.OnChanged += ChangeListener;
        }
    }
    private void Update()
    {
        if(dmgTimer > 0)
        {
            dmgTimer -= Time.deltaTime;
            if(dmgTimer <= 0)
            {
                dmgSum = 0;
                txtDamageTaken.text = "";

            }
        }
    }
    private float dmgTimer;
    private float dmgSum = 0;
    private void ChangeListener(AttributeInt arg1, int val, int oldVal)
    {
        int diff = oldVal - val;
        dmgSum += diff;
        txtDamageTaken.text = dmgSum + "";
        dmgTimer = 1.5f;

        if (val == 0)
        {
            StartCoroutine(FadeOut(.4f));
        }
    }

    private IEnumerator FadeOut(float duration, bool resetOnFinish = false)
    {
        float t = duration;

        yield return new WaitForSeconds(1f);

        while(t >= 0)
        {
            t -= Time.deltaTime;
            grp.alpha = t / duration;
            yield return null;
        }
        c.enabled = false;
        grp.alpha = 1;
        if (resetOnFinish) ResetState();
    }
}
