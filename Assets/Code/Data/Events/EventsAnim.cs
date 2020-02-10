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

public partial class EventData
{
    public class SetBool
    {
        public string name;
        public bool val;
    }
}