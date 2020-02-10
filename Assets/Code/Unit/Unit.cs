using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Events))]
public class Unit : WorldObject
{
    [SerializeField]
    private UnitData _data = default;
    public UnitData data { get; private set; }
    private Events events;
    [HideInInspector]
    public AttributeInt healthPoints = new AttributeInt(1);

    public bool IsAlive { get { return healthPoints.Value > 0; } }

    private void Awake()
    {
        data = _data;

        if (GetComponentInChildren<Animator>() == null)
        {
            var model = Instantiate(data.model, transform);
            model.transform.localScale = new Vector3(data.modelScale.x, data.modelScale.y, 1);
        }
    }
    private void Start()
    {
        events = GetComponent<Events>();
        gameObject.layer = LayerMask.NameToLayer("Unit");
        healthPoints.Reset(data.healthPoints);
        healthPoints.OnChanged += OnHealthChange;
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

    public void SufferDamage(int amount, Unit attacker)
    {
        if (IsAlive)
        {
            events.Emit(Events.Unit.RecieveDamage, attacker);
            events.Emit(Events.Audio.Combat_RecieveHit);

            healthPoints.Value -= amount;
        }
    }

    public void ResetState()
    {
        healthPoints.Value = data.healthPoints;
        

        events.Emit(Events.Unit.ResetState);
    }


    [Range(0, 1)]
    [SerializeField]
    private float _health = 1;

    private void OnValidate()
    {
        this.healthPoints.Value = (int)(this.healthPoints.ValueMax * _health);
    }

}
