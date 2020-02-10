using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private ResourceBar healthBar = default;

    private Unit player;
    private void Start()
    {
        LocateAndLinkPlayer();
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
