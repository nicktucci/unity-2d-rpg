﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Unit))]
public class AIController : UnitController
{
    private Vector3 target;
    private Vector3 posLastFrame;
    public float StopDistance { get; private set; } = 1.5f;

    private bool reachedTarget;
    private bool suspendMovement;


    private MovementState moveState;

    public override UnitDirection Direction => moveState != null ? moveState.direction : UnitDirection.None;
    public float DistanceToTarget { get; private set; }


    private new void Start()
    {
        base.Start();

        ResetState();

        events.Subscribe(Events.Unit.ResetState, e =>
        {
            ResetState();
        });
        events.Subscribe(Events.Unit.SetTarget, e =>
        {
            SetState((Vector3)e.data);
        });

        events.Subscribe(Events.Unit.Death, e =>
        {
            suspendMovement = true;
        });

        events.Subscribe(Events.Unit.SuspendMove, e =>
        {
            suspendMovement = true;
        });
        events.Subscribe(Events.Unit.RestoreMove, e =>
        {
            suspendMovement = false;
        });
    }

    private void ResetState()
    {
        reachedTarget = false;
        suspendMovement = false;
        moveState = new MovementState();
        target = posLastFrame = transform.position;
    }

    public void SetState(Vector3 target)
    {
        this.target = target;
        reachedTarget = false;
        DistanceToTarget = Vector2.Distance(transform.position, target);
    }

    private void Update()
    {
        if (!suspendMovement)
        {
            if (!reachedTarget)
            {
                DistanceToTarget = Vector2.Distance(transform.position, target);
                if (DistanceToTarget > StopDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, runSpeed * Time.deltaTime);
                }
                else
                {
                    DistanceToTarget = Mathf.Abs(target.y - transform.position.y);
                    if (DistanceToTarget > 0.1f)
                    {
                        target.x = transform.position.x;
                        transform.position = Vector3.MoveTowards(transform.position, target, runSpeed * Time.deltaTime);
                    }
                    else
                    {
                        //transform.position = target;
                        DistanceToTarget = 0;
                        reachedTarget = true;
                    }
                }

                UpdateFacingDirection(target);
            }
        }


        UpdateMagnitude();

        events.MoveStateChanged(moveState);

    }
    private void UpdateFacingDirection(Vector3 target)
    {
        if (target.x < transform.position.x)
        {
            moveState.direction = UnitDirection.Left;
        }
        else if (target.x > transform.position.x)
        {
            moveState.direction = UnitDirection.Right;
        }
    }
    private void UpdateMagnitude()
    {
        float magnitude = (transform.position - posLastFrame).magnitude / Time.deltaTime;

        moveState.magnitude = magnitude;

        posLastFrame = transform.position;

    }


    protected override void PreAttack()
    {
        suspendMovement = true;
        UpdateFacingDirection(target);

    }
    protected override void PostAttack()
    {
        suspendMovement = false;

    }
}
