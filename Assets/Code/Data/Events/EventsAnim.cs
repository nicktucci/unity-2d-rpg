using UnityEngine;

public partial class Events
{
    public enum Anim
    {
        PlayDeathAnimation,
        PlayCombatAnimation,
        FaceTarget,
        SetBool,
        SetFloat
    }
}



public class EventAnimBool : GameEvent
{
    public readonly string name;
    public readonly new bool data;

    public EventAnimBool(MonoBehaviour emitter, string name, bool val) : base(emitter, null)
    {
        this.name = name;
        this.data = val;
    }
}


public class EventAnimFloat : GameEvent
{
    public readonly string name;
    public readonly new float data;

    public EventAnimFloat(MonoBehaviour emitter, string name, float val) : base(emitter, null)
    {
        this.name = name;
        this.data = val;
    }
}


