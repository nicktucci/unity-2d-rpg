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
    private Canvas worldResetCanvas = default;

    private Unit player;
    private Coroutine routine;
    private void Start()
    {
        LocateAndLinkPlayer();

        worldResetCanvas = worldResetOverlay.gameObject.AddComponent<Canvas>();
        worldResetCanvas.enabled = false;
        GlobalEvents.Get.Subscribe(Events.Global.Misc.CheckPointReset, d => {
            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(ResetAnim());
        });
    }

    private IEnumerator ResetAnim()
    {
        worldResetOverlay.alpha = 1;
        worldResetCanvas.enabled = true;


        yield return new WaitForSeconds(1f);

        while (worldResetOverlay.alpha > 0)
        {
            worldResetOverlay.alpha -= Time.deltaTime;
            yield return null;
        }
        worldResetOverlay.alpha = 0;
        worldResetCanvas.enabled = true;

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
