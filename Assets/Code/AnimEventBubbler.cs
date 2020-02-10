using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventBubbler : MonoBehaviour
{
    private Events events;
    private void Start()
    {
        events = transform.parent.GetComponent<Events>();
    }
    public void SFX_Footstep()
    {
        events.Emit(Events.Audio.SFX_Footstep);
    }
}
