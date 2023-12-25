using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditDamage : MonoBehaviour
{
    [SerializeField] private Collider swordCollider;

    private void Awake()
    {
        swordCollider.enabled = false;
    }

    public void StartAttack()
    {
        swordCollider.enabled = true;
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
    }
}
