using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveModule
{
    Vector3 DirectionVector { get; }
    void Setup(GameObject owner);
    void Update(float speed, bool movementEnabled, bool inputEnabled);

    UnitDirection Direction { get; }
    float Magnitude { get; }
}
