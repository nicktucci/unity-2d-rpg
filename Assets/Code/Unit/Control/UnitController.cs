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
    private int hitMask;
    private void Awake()
    {
        hitMask = (1 << LayerMask.NameToLayer("Unit")) | (1 << LayerMask.NameToLayer("Collision"));
        
    }
    protected void Start()
    {
        events = GetComponent<Events>();
        unit = GetComponent<Unit>();

        runSpeed = unit.data.speed;
        walkSpeed = runSpeed / 2;
    }


    protected IAttackable[] GetObjectsInMelee(MeleeAttackData data)
    {
        HashSet<IAttackable> res = new HashSet<IAttackable>();

        float dir = (Direction == UnitDirection.Right ? 1 : -1);
        var hits = Physics2D.OverlapBoxAll(GetMeleeHitPosition(data, dir), GetMeleeHitSize(data), 0, hitMask);
        foreach (var hit in hits)
        {
            var i = hit.transform.GetComponent<IAttackable>();
            var cp = hit.transform.GetComponent<ColliderParent>();
            if (i == null && cp != null)
            {
                i = cp.parent.GetComponent<IAttackable>();

            }
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

    private Vector2 GetMeleeHitSize(MeleeAttackData data)
    {
        return (Vector2.right * data.width) + (Vector2.up * data.height);
    }

    private Vector3 GetMeleeHitPosition(MeleeAttackData data, float dir)
    {
        return transform.position + (Vector3.right * data.right  * dir) + (Vector3.up * data.up);
    }

    private void OnDrawGizmos()
    {
        float dir = (Direction == UnitDirection.Right ? 1 : -1);
        
        //Gizmos.DrawWireCube(GetMeleeHitPosition(dir), GetMeleeHitSize());
    }
    protected virtual void PreAttack()
    {
    }
    protected virtual void PostAttack()
    {
    }
    
   

    public virtual IEnumerator DoAttack(MeleeAttackData attack)
    {
        float duration = attack.clip.length;
        float timingBegin = attack.swingBegin / duration;
        float timingTrigger = attack.swingTrigger / duration;
        float timingReleaseControl = attack.swingReleaseControl / duration;

        duration = duration / unit.AnimationScale;
        timingBegin *= duration;
        timingTrigger *= duration;
        timingReleaseControl *= duration;

        Debug.Log(timingBegin);
        Debug.Log(timingTrigger);
        Debug.Log(timingReleaseControl);


        PreAttack();
        events.Emit(Events.Anim.PlayCombatAnimation, GameEvent.Create(this, attack.animationLabel));

        yield return new WaitForSeconds(attack.swingBegin);
        events.Emit(Events.Audio.Combat_Swing);

        yield return new WaitForSeconds(timingTrigger - timingBegin);

        if (unit.IsAlive)
        {
            var hits = GetObjectsInMelee(attack);
            foreach (var hit in hits)
            {
                hit.ReceiveAttack(UnityEngine.Random.Range(6, 12), unit);
                if (hit is Unit) events.Emit(Events.Audio.Combat_Connect);
            }
        }

        yield return new WaitForSeconds(timingReleaseControl - timingTrigger - timingBegin);
        PostAttack();
    }
    public abstract UnitDirection Direction { get; }
}
public enum UnitDirection : sbyte
{
    None = -1,
    Right = 0,
    Left = 1
}