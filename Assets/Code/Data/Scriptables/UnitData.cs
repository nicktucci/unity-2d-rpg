using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/New Unit")]
public class UnitData : ScriptableObject
{
    public new string name;
    public int healthPoints;
    [Header("Model")]
    public GameObject model;
    public Vector2 modelScale = Vector2.one;
    public float speed = 3.5f;

    [Header("Audio")]
    public UnitSounds unitSounds;
    public CombatSounds combatSounds;
    [Header("Effects")]
    public GameObject onHitParticle;
    [Header("Animation")]
    public List<AnimationOverride> animationOverrides = new List<AnimationOverride>();
    private Dictionary<string, string> animOverridesDict;

    [Header("Drops")]
    public UnitDroptable dropTable;

    [Header("Attacks")]
    public MeleeAttackData[] meleeAttacks;
    public MeleeAttackData[] specialAttacks;

    private void OnEnable()
    {
        animOverridesDict = new Dictionary<string, string>();
        foreach (var o in animationOverrides)
        {
            animOverridesDict[o.animation.ToString()] = o.overrideAnim;
        }
    }


    public string GetOverrideAnimation(AnimationLabel anim)
    {
        if (animOverridesDict.ContainsKey(anim.ToString()))
        {
            return animOverridesDict[anim.ToString()];
        }
        return null;
    }
}

[System.Serializable]
public struct AnimationOverride
{
    public AnimationLabel animation;
    public string overrideAnim;
}
[System.Serializable]
public struct UnitDroptable
{
    public List<GameObject> items;
    public float dropChance;
}
public enum AnimationLabel
{
    Attack_1,
    Attack_2,
    Death
}