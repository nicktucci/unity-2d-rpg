using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : UnitController
{

    private Rigidbody2D rb;
    IMoveModule moveModule;
    public Vector3 MoveDirection { get => moveModule != null ? moveModule.DirectionVector : Vector3.zero; }
    public float Magnitude { get => moveModule != null ? moveModule.Magnitude : 0; }

    public override UnitDirection Direction => moveModule != null ? moveModule.Direction : UnitDirection.None;

    public bool hasInputFieldSelection;


    public Coroutine attackRoutine;
    private bool controlsLocked;
    private bool IsAttacking { get => attackRoutine != null; }
    private float deathTimer = 0;

    public void SetInputType(int type)
    {

        moveModule = new DirectionMover();

        moveModule.Setup(this.gameObject);
    }

    new void Start()
    {
        base.Start();

        ResetState();


        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.hideFlags = HideFlags.HideInInspector;

        moveModule = new DirectionMover();
        moveModule.Setup(this.gameObject);

        events.Subscribe(Events.Unit.ResetState, data => {
            ResetState();
        });

        events.Subscribe(Events.Unit.Death, data =>
        {
            deathTimer = 2f;
            controlsLocked = true;
            GlobalEvents.Get.Emit(Events.Global.Misc.PlayerDeath);

            IsBlocking = false;
            events.Emit(Events.Anim.SetBool, new EventData.SetBool() { name = "IsBlocking", val = IsBlocking });
        });

        gameObject.tag = "Player";
    }

    private void ResetState()
    {
        controlsLocked = false;
    }

    void Update()
    {
        if (EventSystem.current != null)
        {
            hasInputFieldSelection = EventSystem.current.currentSelectedGameObject?.GetComponent<UnityEngine.UI.InputField>() != null || EventSystem.current.currentSelectedGameObject?.GetComponent<TMPro.TMP_InputField>() != null;
        }
        else
        {
            hasInputFieldSelection = false;
        }

        float speed = IsBlocking ? runSpeed * .75f : runSpeed;
        moveModule.Update(speed, !controlsLocked, !hasInputFieldSelection);
        events.Emit(new MovementState() { 
            direction = moveModule.Direction, 
            magnitude = moveModule.Magnitude / runSpeed
        });

        if (!controlsLocked && !hasInputFieldSelection)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!IsAttacking && !IsBlocking)
                    attackRoutine = StartCoroutine(DoAttack());
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!IsAttacking && !IsBlocking)
                    GlobalEvents.Get.Emit(Events.Global.UI.ActivateInteractable, unit);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (unit.IsAlive && !IsAttacking)
                {
                    IsBlocking = true;
                    events.Emit(Events.Anim.SetBool, new EventData.SetBool() { name = "IsBlocking", val = IsBlocking });
                }

            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                IsBlocking = false;
                events.Emit(Events.Anim.SetBool, new EventData.SetBool() { name = "IsBlocking", val = IsBlocking });
            }
        }

        if(deathTimer > 0)
        {
            deathTimer -= Time.deltaTime;
            if(deathTimer <= 0)
            {
                deathTimer = 0;
            }
        }
        if(!unit.IsAlive && deathTimer == 0)
        {
            if (Input.anyKeyDown)
            {
                FindObjectOfType<Checkpoint>().Restore();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var i = other.GetComponent<IInteractable>();
        if (i != null) GlobalEvents.Get.Emit(Events.Global.UI.QueueInteractable, i);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        var i = other.GetComponent<IInteractable>();
        if (i != null) GlobalEvents.Get.Emit(Events.Global.UI.DequeueInteractable, i);
    }
    private IEnumerator DoAttack()
    {
        float rnd = UnityEngine.Random.Range(0, 1f);
        string animLabel = rnd > .5f ? "Attack_1" : "Attack_2";

        controlsLocked = true;
        events.Emit(Events.Anim.PlayCombatAnimation, animLabel);

        yield return new WaitForSeconds(0.20f);
        events.Emit(Events.Audio.Combat_Swing);

        yield return new WaitForSeconds(0.20f);

        if (unit.IsAlive)
        {
            var hits = GetUnitsInMelee();
            foreach (var hit in hits)
            {
                hit.SufferDamage(UnityEngine.Random.Range(6,12), this.GetComponent<Unit>());
                events.Emit(Events.Audio.Combat_Connect);
            }
        }

        yield return new WaitForSeconds(0.30f);

        attackRoutine = null;
        if (unit.IsAlive)
        {
            controlsLocked = false;
        }

    }

    


}