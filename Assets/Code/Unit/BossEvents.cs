using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvents : MonoBehaviour
{
    private Events events;

    private void Start()
    {
        gameObject.tag = "Boss";
        events = GetComponent<Events>();

        events.Subscribe(Events.Unit.EnterCombat, e =>
        {
            if(((Unit) e.data).gameObject.tag == "Player")
            {
                GlobalEvents.Get.Emit(Events.Global.Misc.TriggerBossArea);
            }
        });
    }
}
