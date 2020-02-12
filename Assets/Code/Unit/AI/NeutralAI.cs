using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIController))]
[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(Events))]
public class NeutralAI : MonoBehaviour
{
    protected Events events;
    protected AIController controller;
    protected Unit unit;
    protected Coroutine routine;

    protected Unit target;

    protected const float aiCooldown = 0.25f;
    private void Start()
    {
        events = GetComponent<Events>();
        controller = GetComponent<AIController>();
        unit = GetComponent<Unit>();

        events.Subscribe(Events.Unit.RecieveDamage, e =>
        {
            if(target == null)
            {
                events.Emit(Events.Unit.EnterCombat, GameEvent.Create(this, e.data));
                target = (Unit)e.data;
            }
        });

        events.Subscribe(Events.Unit.ResetState, attacker =>
        {
            ResetState();
        });

        ResetState();
    }
    protected virtual IEnumerator AI()
    {
        while (unit.IsAlive)
        {
            if(target && target.IsAlive)
            {
                yield return FollowAndAttack(target);
                CycleTargets();
            }
            yield return new WaitForSeconds(aiCooldown);
        }
    }

    protected virtual void CycleTargets()
    {
        if (!target.IsAlive) target = null;
    }

    protected virtual IEnumerator FollowAndAttack(Unit target)
    {
        events.Emit(Events.Unit.SetTarget, GameEvent.Create(this, target.transform.position));
        if (controller.DistanceToTarget <= controller.StopDistance)
        {
            yield return StartCoroutine(ChooseAttack());
        }
    }

    protected virtual IEnumerator ChooseAttack()
    {
        return controller.DoAttack(ChooseMeleeAttackData());
    }

    protected virtual MeleeAttackData ChooseMeleeAttackData()
    {
        return unit.data.meleeAttacks[Random.Range(0, unit.data.meleeAttacks.Length)];
    }

    public virtual void ResetState()
    {
        if(routine != null)
        {
            StopCoroutine(routine);
        }
        target = null;
        StartAI();

        routine = StartCoroutine(AI());

    }

    protected virtual void StartAI()
    {
    }
}

