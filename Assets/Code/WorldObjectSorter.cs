using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WorldObjectSorter : MonoBehaviour {
    private static WorldObjectSorter instance;
    [SerializeField]
    private WorldObject[] worldObjects;
    private static bool dirty;
    private void Start()
    {
        UpdateObjects();
    }

    private void RunInEditMode()
    {
        instance = this;
        UpdateObjects();

        Update();

        worldObjects = null;
        instance = null;
    }
    void Update () {
        
        foreach(var o in worldObjects)
        {
            if(o != null && o.isActiveAndEnabled)
            {
                if(o.GetComponentInChildren<SortingGroup>() != null)
                {
                    Sort(o, o.GetComponentInChildren<SortingGroup>());
                } else if(o.GetComponentInChildren<Renderer>() != null)
                {
                    Sort(o, o.GetComponentInChildren<Renderer>());
                }

            } else
            {
                dirty = true;
            }
        }

        if (dirty)
        {
            UpdateObjects();
        }
    }

    void Sort(WorldObject t, SortingGroup s)
    {
        float posY = t.transform.position.y;
        if (t.gameObject.tag == "Player") posY *= 1.025f;
        var order = Mathf.RoundToInt(posY * 100f) * -1; ;
        s.sortingOrder = order;
        t.sortingIndex = order;
    }

    void Sort(WorldObject t, Renderer s)
    {
        float posY = t.transform.position.y;
        if (t.gameObject.tag == "Player") posY *= 1.025f;
        var order = Mathf.RoundToInt(posY * 100f) * -1; ;
        s.sortingOrder = order;
        t.sortingIndex = order;
    }

    private void UpdateObjects()
    {
        worldObjects = Transform.FindObjectsOfType<WorldObject>();
    }
    public static void SetDirty()
    {
        dirty = true;
    }
}
