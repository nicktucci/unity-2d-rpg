using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LootableObject : WorldObject, IInteractable
{
    public string InteractLabel { get => $"Take {name}";}
    public new string name = "Object";

    public void Interact(Unit user)
    {
        Debug.Log($"Player looted {name}");
        Destroy(this.gameObject);
    }
}
