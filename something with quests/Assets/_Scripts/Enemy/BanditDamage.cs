using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditDamage : MonoBehaviour
{
    [SerializeField] private Collider swordCollider;
    
    private bool _isAttackingSfx;
    
    [Header("Audio")] 
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip swordSwingSfx;
    [SerializeField] private AudioClip gruntSfx;

    private void Awake()
    {
        swordCollider.enabled = false;
    }

    public void StartAttack()
    {
        swordCollider.enabled = true;
        if (!_isAttackingSfx)
        {
            audioSource.PlayOneShot(swordSwingSfx);
            audioSource.PlayOneShot(gruntSfx);
        }

        _isAttackingSfx = true;
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
        _isAttackingSfx = false;
    }
}
