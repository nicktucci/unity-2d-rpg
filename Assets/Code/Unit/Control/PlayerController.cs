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

    private bool hasInputFieldSelection;


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
            events.Emit(Events.Anim.SetBool, new EventAnimBool(this, "IsBlocking", IsBlocking));
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
                {
                    var attack = unit.data.meleeAttacks[Random.Range(0, unit.data.meleeAttacks.Length)];
                    attackRoutine = StartCoroutine(DoAttack(attack));
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!IsAttacking && !IsBlocking)
                    GlobalEvents.Get.Emit(Events.Global.UI.ActivateInteractable, GameEvent.Create(this, unit));
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (unit.IsAlive && !IsAttacking)
                {
                    IsBlocking = true;
                    events.Emit(Events.Anim.SetBool, new EventAnimBool(this, "IsBlocking", IsBlocking));
                }

            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                IsBlocking = false;
                events.Emit(Events.Anim.SetBool, new EventAnimBool(this, "IsBlocking", IsBlocking));
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
        if (i != null) GlobalEvents.Get.Emit(Events.Global.UI.QueueInteractable, GameEvent.Create(this, i));
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        var i = other.GetComponent<IInteractable>();
        if (i != null) GlobalEvents.Get.Emit(Events.Global.UI.DequeueInteractable, GameEvent.Create(this, i));
    }

    protected override void PreAttack()
    {
        controlsLocked = true;
    }
    protected override void PostAttack()
    {
        attackRoutine = null;
        if (unit.IsAlive)
        {
            controlsLocked = false;
        }
    }



}