using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(Events))]
public class UnitAnimator : MonoBehaviour
{
    private Events events;
    private Unit unit;

    private Animator anim;

    private float combatIdleTime = 0;
    private float recoilCooldown = 0;

    private Dictionary<string, AnimationClip> clipLibrary;


    private void Update()
    {
        UpdateIdle();
        if (recoilCooldown > 0) recoilCooldown -= Time.deltaTime;
    }



    private void Start()
    {
        events = GetComponent<Events>();
        unit = GetComponent<Unit>();
        anim = GetComponentInChildren<Animator>();

        anim.gameObject.AddComponent<AnimEventBubbler>();

        UpdateAnimationLibrary();

        events.MoveStateChanged += UpdateMovementState;

        events.Subscribe(Events.Anim.PlayDeathAnimation, e =>
        {
            string label = unit.data.GetOverrideAnimation(AnimationLabel.Death) ?? "Death";
            anim.Play(label);

        });
        events.Subscribe(Events.Anim.PlayCombatAnimation, e =>
        {
            if (unit.IsAlive)
            {
                string label = (string)e.data;
                anim.CrossFadeInFixedTime(label, 0.05f, -1, 0);
                combatIdleTime = 2.5f;
            }
        });
        events.Subscribe(Events.Anim.FaceTarget, e =>
        {
            FaceTarget((Transform)e.data);
        });

        events.Subscribe(Events.Unit.RecieveDamage, data =>
        {
            if(recoilCooldown <= 0)
            {
                anim.SetTrigger("Recoil");
                recoilCooldown = 1.25f;
            }
        });
        events.Subscribe(Events.Unit.ResetState, o =>
        {
            combatIdleTime = 0;
            recoilCooldown = 0;

            // Resets all animator variables
            anim.Rebind();

            anim.SetFloat("Speed", unit.AnimationScale);
        });
        events.Subscribe(Events.Anim.SetBool, e =>
        {
            var a = (EventAnimBool)e;
            anim.SetBool(a.name, a.data);
        });
        events.Subscribe(Events.Anim.SetFloat, e =>
        {
            var a = (EventAnimFloat)e;

            anim.SetFloat(a.name, a.data);
        });
    }

    private void UpdateAnimationLibrary()
    {
        clipLibrary = new Dictionary<string, AnimationClip>();
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            clipLibrary[clip.name] = clip;
        }
    }

    

    protected void UpdateIdle()
    {
        if (combatIdleTime > 0)
        {
            anim.SetFloat("AmountCombatIdle", 1, 0.07f, Time.deltaTime);
            combatIdleTime -= Time.deltaTime;
        }
        else
        {
            anim.SetFloat("AmountCombatIdle", 0, 0.07f, Time.deltaTime);
        }
    }
    public void FaceDirection(UnitDirection dir)
    {
        if (dir == UnitDirection.Left)
        {
            anim.transform.localScale = new Vector3(Mathf.Abs(anim.transform.localScale.x), anim.transform.localScale.y, anim.transform.localScale.z);

        }
        else if (dir == UnitDirection.Right)
        {
            anim.transform.localScale = new Vector3(-Mathf.Abs(anim.transform.localScale.x), anim.transform.localScale.y, anim.transform.localScale.z);
        }
    }
    public void FaceTarget(Transform target)
    {
        if(target.transform.position.x > transform.position.x)
        {
            FaceDirection(UnitDirection.Right);
        } else
        {
            FaceDirection(UnitDirection.Left);

        }
    }
    private float smoothMagnitude = 0;
    private void UpdateMovementState(MovementState state)
    {
        smoothMagnitude = Mathf.MoveTowards(smoothMagnitude, state.magnitude, 6f * Time.deltaTime);
        anim.SetFloat("Magnitude", smoothMagnitude);
        FaceDirection(state.direction);
    }
}
