using UnityEngine;

public class SwordHit : MonoBehaviour
{
    [SerializeField] private LayerMask layerToHit;
    [SerializeField] private FloatReference damageAmount;
    private float _maxDistance = 2.0f;
    private bool _isHit;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip swordHitSfx;
    
    
    public void SendHitRaycast()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.white, 3.0f, true);
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, layerToHit) && !_isHit)
        {
            _isHit = true;
            BanditEnemy banditEnemy = hit.collider.GetComponent<BanditEnemy>();

            if (banditEnemy != null)
            {
                banditEnemy.TakeDamage(damageAmount);
            }
            audioSource.PlayOneShot(swordHitSfx);
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
            audioSource.PlayOneShot(swordHitSfx);

        }
    }

    public void ResetIsHit()
    {
        _isHit = false;
    }
}
