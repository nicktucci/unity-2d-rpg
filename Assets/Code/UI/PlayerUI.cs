using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private ResourceBar healthBar = default;
    [SerializeField]
    private CanvasGroup worldResetOverlay = default;

    private Unit player;
    private void Start()
    {
        LocateAndLinkPlayer();

        worldResetOverlay.alpha = 0;
        GlobalEvents.Get.Subscribe(Events.Global.Misc.CheckPointReset, d => {
            StartCoroutine(ResetAnim());
        });
    }

    private IEnumerator ResetAnim()
    {
        worldResetOverlay.alpha = 1;

        while (worldResetOverlay.alpha > 0)
        {
            worldResetOverlay.alpha -= Time.deltaTime;
            yield return null;
        }
        worldResetOverlay.alpha = 0;

    }

    private void LateUpdate()
    {
        if (player == null) LocateAndLinkPlayer();
    }

    private void LocateAndLinkPlayer()
    {
        var p = FindObjectOfType<PlayerController>();
        player = p.GetComponent<Unit>();

        healthBar.Link(player.healthPoints);
    }
}
