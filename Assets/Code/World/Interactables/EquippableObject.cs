using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Events))]
public class EquippableObject : WorldObject, IInteractable
{
    public string InteractLabel { get => $"Equip {name}";}
    public new string name = "Object";
    public GameObject equipObject;
    public EquipSlot slot;

    public void Interact(Unit user)
    {
        Destroy(this.gameObject);
        user.GetComponent<Events>().Emit(Events.Unit.EquipObject, new EquipableEvent(this, equipObject, slot));
    }
    public enum EquipSlot
    {
        Armor,
        Weapon
    }
    public class EquipableEvent : GameEvent
    {
        public new GameObject data;
        public EquipSlot slot;
        public EquipableEvent(MonoBehaviour emitter, GameObject obj, EquipSlot slot) : base(emitter, null)
        {
            data = obj;
            this.slot = slot;
        }
    }
}
