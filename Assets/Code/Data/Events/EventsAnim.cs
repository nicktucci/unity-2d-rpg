using UnityEngine;

public partial class Events
{
    public enum Anim
    {
        PlayDeathAnimation,
        PlayCombatAnimation,
        FaceTarget,
        SetBool
    }
}



public class AnimBoolEvent : GameEvent
{
    public readonly string name;
    public readonly new bool data;

    public AnimBoolEvent(MonoBehaviour emitter, string name, bool val) : base(emitter, null)
    {
        this.name = name;
        this.data = val;
    }
}
