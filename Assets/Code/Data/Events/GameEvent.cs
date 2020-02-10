

using UnityEngine;

public class GameEvent
{
    public readonly object data;
    public readonly MonoBehaviour emitter;

    public GameEvent(MonoBehaviour emitter, object data)
    {
        this.emitter = emitter;
        this.data = data;
    }

    public static GameEvent Create(MonoBehaviour emitter, object data)
    {
        return new GameEvent(emitter, data);
    }
}