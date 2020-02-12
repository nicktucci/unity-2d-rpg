using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, IInteractable
{
    private List<Unit> units = new List<Unit>();
    private Dictionary<Unit, UnitState> unitStates = new Dictionary<Unit, UnitState>();

    public string InteractLabel => "Rest at bonfire";

    private void Start()
    {
        GetAreaEntities();

        CacheOriginalStates();
    }

    private void CacheOriginalStates()
    {
        foreach (var unit in units)
        {
            unitStates[unit] = new UnitState() { position = unit.transform.position, scale = unit.transform.localScale, rot = unit.transform.rotation };
        }
    }

    public void Restore(bool ignorePlayerPosition = false)
    {
        foreach (var kvp in unitStates)
        {
            var u = kvp.Key;
            
            u.transform.localScale = kvp.Value.scale;
            if(u.gameObject.tag != "Player" || !ignorePlayerPosition)
            {
                u.transform.position = kvp.Value.position;
                u.transform.rotation = kvp.Value.rot;
            }


            u.ResetState();
        }

        GlobalEvents.Get.Emit(Events.Global.Misc.CheckPointReset);
    }

    private void GetAreaEntities()
    {
        var cols = GetComponents<BoxCollider2D>();
        BoxCollider2D col = null;
        foreach (var c in cols)
        {
            if (c.isTrigger) continue;
            col = c;
        }
        if (col == null) throw new Exception("Colliders improperly setup on CheckPoint. Area collider should not be a trigger, second collider for interact approach SHOULD be a trigger.");
        var hits = Physics2D.OverlapBoxAll(col.transform.position, col.size, 0);
        foreach (var hit in hits)
        {
            Unit u = hit.GetComponent<Unit>();
            if (u != null)
            {
                units.Add(u);
            }
        }

        col.enabled = false;
        Destroy(col);
    }

    public void Interact(Unit user)
    {
        Restore(true);
    }

    private class UnitState
    {
        public Vector3 scale;
        public Vector3 position;
        public Quaternion rot;
    }
}

