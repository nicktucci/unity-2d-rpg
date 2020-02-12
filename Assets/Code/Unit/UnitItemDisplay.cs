using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitItemDisplay : MonoBehaviour
{
    Transform model;

    private const string PATH_WEAPON = "WeaponF";
    private const string PATH_BODY = "body";
    private const string PATH_TABARD = PATH_BODY + "/TabardF";
    private const string PATH_HEAD = PATH_BODY + "/Head";
    private const string PATH_HELM = PATH_HEAD + "/helm";
    private const string PATH_SHOULDER = PATH_BODY + "/Shoulder";
    private const string PATH_LEG = "Leg";


    private const string ARMORSET_PATH_BODY = "Body";
    private const string ARMORSET_PATH_TABARD =   "Tabard";
    private const string ARMORSET_PATH_HEAD =   "Head";
    private const string ARMORSET_PATH_HELM =  "Helm";
    private const string ARMORSET_PATH_SHOULDER =  "Shoulder";
    private const string ARMORSET_PATH_LEG = "Leg";
    private readonly string[] allPaths = new string[] { PATH_WEAPON, PATH_BODY, PATH_TABARD, PATH_HEAD, PATH_HELM, PATH_SHOULDER + "F", PATH_SHOULDER + "B", PATH_LEG + "F", PATH_LEG + "B" };

    public GameObject armorSetDefault;
    public GameObject weaponDefault;

    [Space]
    public GameObject armorSet1;
    public GameObject weapon1;
    [Space]
    public GameObject armorSet2;
    public GameObject weapon2;
    [Space]
    public GameObject armorSet3;
    public GameObject weapon3;

    private Dictionary<string, Transform> cache = new Dictionary<string, Transform>();
    private Dictionary<Transform, Transform> slotClones = new Dictionary<Transform, Transform>();


    private void Start()
    {
        model = GetComponentInChildren<Animator>().transform;

        CacheAll();
        DestroyDefaults();

        LoadArmorObject(armorSetDefault);
        LoadSlot(cache[PATH_WEAPON], weaponDefault);
    }

    private void LoadArmorObject(GameObject armor)
    {
        GameObject helm = armor.transform.Find(ARMORSET_PATH_HELM)?.gameObject;
        GameObject tabard = armor.transform.Find(ARMORSET_PATH_TABARD)?.gameObject;
        GameObject head = armor.transform.Find(ARMORSET_PATH_HEAD)?.gameObject;
        GameObject shoulder = armor.transform.Find(ARMORSET_PATH_SHOULDER)?.gameObject;
        GameObject body = armor.transform.Find(ARMORSET_PATH_BODY)?.gameObject;
        GameObject leg = armor.transform.Find(ARMORSET_PATH_LEG)?.gameObject;

        LoadSlot(cache[PATH_HELM], helm);
        LoadSlot(cache[PATH_TABARD], tabard);
        LoadSlot(cache[PATH_HEAD], head);
        LoadSlot(cache[PATH_BODY], body);
        LoadSlot(cache[PATH_SHOULDER + "F"], shoulder);
        LoadSlot(cache[PATH_SHOULDER + "B"], shoulder, -1);
        LoadSlot(cache[PATH_LEG + "F"], leg);
        LoadSlot(cache[PATH_LEG + "B"], leg);
    }

    private void LoadSlot(Transform transform, GameObject obj, float dir = 1)
    {
        if(slotClones.ContainsKey(transform))
        {
            Destroy(slotClones[transform].gameObject);
            slotClones.Remove(transform);
        }

        if(obj != null)
        {
            var clone = Instantiate(obj, transform);
            clone.transform.localPosition = Vector3.zero;
            clone.transform.localScale = new Vector3(dir, 1, 1);
            clone.transform.localRotation = Quaternion.identity;
            slotClones[transform] = clone.transform;
        }

    }

    private void CacheAll()
    {
        foreach (var path in allPaths)
        {
            cache[path] = model.Find(path);
        }
    }
    private void DestroyDefaults()
    {
        foreach (var path in allPaths)
        {
            Destroy(cache[path].GetComponent<SpriteRenderer>());
        }
    }
}
