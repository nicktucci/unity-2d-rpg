using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableObject : MonoBehaviour, IAttackable
{
    public bool IsValidTarget { get; private set; } = true;
    private AudioSource src;

    [SerializeField]
    private AudioClip hitClip = default;
    [SerializeField]
    private GameObject hitParticles = default;

    [SerializeField]
    [Range(0,1)]
    private float volume = 1f;
    private void Start()
    {
        AddAudioSource();
    }

    public void ReceiveAttack(int damage, Unit attacker)
    {
        if (hitClip != null) src.PlayOneShot(hitClip, volume);
        if(hitParticles != null)
        {
            var go = Instantiate(hitParticles, transform.parent);
            go.transform.position = transform.position + (Vector3.up * 0.5f);
            Destroy(go, 10);
        }
    }

    private void AddAudioSource()
    {
        src = gameObject.AddComponent<AudioSource>();
        src.hideFlags = HideFlags.HideInInspector;
    }
}
