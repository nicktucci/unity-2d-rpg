using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Events))]
public abstract class UnitController : MonoBehaviour
{
    protected Events events;
    protected Unit unit;

    protected float runSpeed = 3.0f;
    protected float walkSpeed = 1.5f;
    public bool IsBlocking { get; protected set; }
    protected void Start()
    {
        events = GetComponent<Events>();
        unit = GetComponent<Unit>();

        runSpeed = unit.data.speed;
        walkSpeed = runSpeed / 2;
    }


    readonly float meleeSwingBoxRight = 0.61f;
    readonly float meleeSwingBoxUp = .5f;
    readonly float meleeSwingBoxWidth = 1.5f;
    readonly float meleeSwingBoxHeight = 1f;
    protected IAttackable[] GetObjectsInMelee()
    {
        HashSet<IAttackable> res = new HashSet<IAttackable>();

        float dir = (Direction == UnitDirection.Right ? 1 : -1);
        var hits = Physics2D.OverlapBoxAll(GetMeleeHitPosition(dir), GetMeleeHitSize(), 0);
        foreach (var hit in hits)
        {
            var i = hit.transform.GetComponent<IAttackable>();
            if (i != null && i.IsValidTarget && IsTargetInRealisticY(hit.transform.position))
            {
                if (hit.gameObject == this.gameObject) continue;
                res.Add(i);
            }
        }
        var final = new IAttackable[res.Count];
        res.CopyTo(final);
        return final;
    }

    private bool IsTargetInRealisticY(Vector3 position)
    {
        return Mathf.Abs(transform.position.y - position.y) < 0.26f;
    }

    private Vector2 GetMeleeHitSize()
    {
        return (Vector2.right * meleeSwingBoxWidth) + (Vector2.up * meleeSwingBoxHeight);
    }

    private Vector3 GetMeleeHitPosition(float dir)
    {
        return transform.position + (Vector3.right * meleeSwingBoxRight * dir) + (Vector3.up * meleeSwingBoxUp);
    }

    private void OnDrawGizmos()
    {
        float dir = (Direction == UnitDirection.Right ? 1 : -1);
        
        Gizmos.DrawWireCube(GetMeleeHitPosition(dir), GetMeleeHitSize());
    }

    public abstract UnitDirection Direction { get; }
}
public enum UnitDirection : sbyte
{
    None = -1,
    Right = 0,
    Left = 1
}