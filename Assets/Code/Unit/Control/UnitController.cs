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


    readonly float meleeSwingBoxRight = 1.05f;
    readonly float meleeSwingBoxSize = 1.05f;
    readonly float meleeSwingBoxHeight = .7f;
    protected Unit[] GetUnitsInMelee()
    {
        HashSet<Unit> res = new HashSet<Unit>();
        var mask = 1 << LayerMask.NameToLayer("Unit");

        float dir = (Direction == UnitDirection.Right ? 1 : -1);
        var hits = Physics2D.OverlapBoxAll(transform.position + (Vector3.right * meleeSwingBoxRight * dir) + Vector3.up, Vector2.one * meleeSwingBoxSize, 0, mask);
        foreach (var hit in hits)
        {
            var udata = hit.transform.GetComponent<Unit>();
            if (udata != null && udata.IsAlive)
            {
                res.Add(udata);
            }
        }
        var final = new Unit[res.Count];
        res.CopyTo(final);
        return final;
    }

    private void OnDrawGizmos()
    {
        float dir = (Direction == UnitDirection.Right ? 1 : -1);
        
        Gizmos.DrawWireCube(transform.position + (Vector3.right * meleeSwingBoxRight * dir) + (Vector3.up * meleeSwingBoxHeight), Vector2.one * meleeSwingBoxSize);
    }

    public abstract UnitDirection Direction { get; }
}
public enum UnitDirection : sbyte
{
    None = -1,
    Right = 0,
    Left = 1
}