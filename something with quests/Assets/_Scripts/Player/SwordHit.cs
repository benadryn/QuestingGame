using System;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    [SerializeField] private LayerMask layerToHit;
    [SerializeField] private float damageMulti;
    [SerializeField] private float maxDistance = 2.0f;
    private float _damage;
    private bool _isHit;

    private BanditEnemy _banditEnemy;

    [Header("Audio")]
    private AudioSource _swordHitAudioSource;
    [SerializeField] private AudioClip swordHitSfx;


    private void Start()
    {
        _swordHitAudioSource = GetComponent<AudioSource>();
    }
    

    public void SendHitRaycast(float damageAmount)
    {
        _damage = damageAmount * damageMulti;
        var transform1 = transform;
        var forward = transform1.forward;
        Debug.DrawRay(transform1.position, forward, Color.white, 3.0f, true);
        Ray ray = new Ray(transform.position, forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerToHit) && !_isHit)
        {
            _isHit = true;
            _banditEnemy = hit.collider.GetComponent<BanditEnemy>();

            if (_banditEnemy != null)
            {
                _banditEnemy.TakeDamage(_damage);
            }
            _swordHitAudioSource.PlayOneShot(swordHitSfx);
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
                banditEnemy.TakeDamage(_damage);
            }
            _swordHitAudioSource.PlayOneShot(swordHitSfx);

        }
    }

    public void ResetIsHit()
    {
        _isHit = false;
    }
}
