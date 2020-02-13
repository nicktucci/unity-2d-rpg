using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(Events))]
public class UnitParticles : MonoBehaviour
{
    private Events events;
    private Unit u;

    private void Start()
    {
        events = GetComponent<Events>();
        u = GetComponent<Unit>();

        events.Subscribe(Events.Unit.RecieveDamage, e =>
        {
            var go = Instantiate(u.data.onHitParticle, transform.parent);
            go.transform.position = transform.position + (Vector3.up * 0.5f);
            Destroy(go, 10);
        });
    }
}
