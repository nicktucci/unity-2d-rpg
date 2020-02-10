using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
     string InteractLabel { get; }
     void Interact(Unit user);
}
