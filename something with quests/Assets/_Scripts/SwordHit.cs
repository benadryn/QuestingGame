using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    [SerializeField] private LayerMask _layerToHit;
    [SerializeField] private FloatReference damageAmount;
    private float maxDistance = 2.0f;
    private bool _isHit;

    public void SendHitRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.white, 3.0f, true);
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, _layerToHit) && !_isHit)
        {
            _isHit = true;
            BanditEnemy banditEnemy = hit.collider.GetComponent<BanditEnemy>();

            if (banditEnemy != null)
            {
                banditEnemy.TakeDamage(damageAmount);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy") && !_isHit)
        {
            _isHit = true;
            BanditEnemy banditEnemy = other.gameObject.GetComponent<BanditEnemy>();
    
            if (banditEnemy != null)
            {
                banditEnemy.TakeDamage(damageAmount);
            }
        }
    }

    public void ResetIsHit()
    {
        _isHit = false;
    }
}
