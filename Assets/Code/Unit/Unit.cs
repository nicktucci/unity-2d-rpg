using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Events))]
public sealed class Unit : WorldObject, IAttackable
{
    [SerializeField]
    private UnitData _data = default;
    public UnitData data { get; private set; }
    private Events events;
    [HideInInspector]
    public AttributeInt healthPoints = new AttributeInt(1);

    public bool IsAlive { get { return healthPoints.Value > 0; } }

    private float animSpeed;
    public float AnimationScale { get => animSpeed; set {
            animSpeed = value;
            events.Emit(Events.Anim.SetFloat, new EventAnimFloat(this, "Speed", animSpeed));
    } }
    
    public bool IsValidTarget => IsAlive;

    private void Awake()
    {
        data = _data;

        if (GetComponentInChildren<Animator>() == null)
        {
            var model = Instantiate(data.model, transform);
            model.transform.localScale = new Vector3(data.modelScale.x, data.modelScale.y, 1);
        }
    }
    private IEnumerator Start()
    {
        events = GetComponent<Events>();
        gameObject.layer = LayerMask.NameToLayer("Unit");
        healthPoints.Reset(data.healthPoints);
        healthPoints.OnChanged += OnHealthChange;

        yield return null;
        if(animSpeed == 0)
        {
            AnimationScale = 1f;
        }
    }

    private void OnHealthChange(AttributeInt attr, int v, int ov)
    {
        if(v == 0)
        {
            events.Emit(Events.Unit.Death);
            events.Emit(Events.Anim.PlayDeathAnimation);

            OnDeath();
        }

        _health = (float)healthPoints.Value / healthPoints.ValueMax;

    }

    private void OnDeath()
    {
        if(data.dropTable.items.Count > 0)
        {
            if(Random.Range(0f, 1f) <= data.dropTable.dropChance)
            {
                var item = data.dropTable.items[Random.Range(0, data.dropTable.items.Count)];
                var drop = Instantiate(item);
                drop.transform.position = this.transform.position;
            }
        }
    }

    public void ResetState()
    {
        healthPoints.Value = data.healthPoints;
        AnimationScale = 1f;

        events.Emit(Events.Unit.ResetState);
    }


    [Range(0, 1)]
    [SerializeField]
    private float _health = 1;

    private void OnValidate()
    {
        this.healthPoints.Value = (int)(this.healthPoints.ValueMax * _health);
    }

    public void ReceiveAttack(int damage, Unit attacker)
    {
        if (IsAlive)
        {
            events.Emit(Events.Unit.RecieveDamage, GameEvent.Create(this, attacker));
            events.Emit(Events.Audio.Combat_RecieveHit);

            healthPoints.Value -= damage;
        }
    }
}
