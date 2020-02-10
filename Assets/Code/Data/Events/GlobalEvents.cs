using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvents : Events
{
    public static GlobalEvents Get { get; private set; }

    private void Awake()
    {
        if (Get != null) throw new System.Exception("GlobalEvents was created more than once. This is a single instance class.");
        Get = this;
    }



}
