using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Attack Data/Melee")]
public class MeleeAttackData : ScriptableObject
{
    public string animationLabel;
    public AnimationClip clip;
    [Header("Timing")]
    public float swingBegin = 0.20f;
    public float swingTrigger = 0.20f;
    public float swingReleaseControl = 0.70f;


    [Header("Position")]
    public float right = 0.61f;
    public float up = .5f;
    public float width = 1.5f;
    public float height = 1f;



}
