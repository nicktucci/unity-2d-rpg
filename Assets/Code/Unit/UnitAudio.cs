using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(Events))]
public class UnitAudio : MonoBehaviour
{
    private Unit unit;
    private AudioSource src;

    protected Events events;


    protected void Start()
    {
        unit = GetComponent<Unit>();
        events = GetComponent<Events>();
        AddAudioSource();

        events.Subscribe(Events.Audio.Combat_Swing, data =>
        {
            PlayClip(unit.data.combatSounds.swingBegin, 0.2f);
        });
        events.Subscribe(Events.Audio.Combat_Connect, data =>
        {
            PlayClip(unit.data.combatSounds.swingConnect);

        });
        events.Subscribe(Events.Audio.Combat_RecieveHit, data =>
        {
            PlayClip(unit.data.combatSounds.recieveHit);
        });
        events.Subscribe(Events.Audio.SFX_Footstep, data =>
        {
            PlayClip(unit.data.unitSounds.footstep, 0.1f);
        });
        events.Subscribe(Events.Unit.Death, data =>
        {
            PlayClip(unit.data.unitSounds.death, 1f);
        });
    }
    private void PlayClip(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        src.PlayOneShot(clip, volume);

    }
    private void AddAudioSource()
    {
        src = gameObject.AddComponent<AudioSource>();
        src.hideFlags = HideFlags.HideInInspector;
    }
}
