using UnityEngine;

public class BanditDamage : MonoBehaviour
{
    [SerializeField] private Collider swordCollider;
    
    private bool _isAttackingSfx;
    private Animator _animator;
    
    private static readonly int Damaged = Animator.StringToHash("Damaged");

    
    [Header("Audio")] 
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip swordSwingSfx;
    [SerializeField] private AudioClip gruntSfx;

    private void Awake()
    {
        swordCollider.enabled = false;
        _animator = GetComponent<Animator>();
    }

    public void StartAttack()
    {
        if (!_animator.GetBool(Damaged))
        {
            if (!_isAttackingSfx)
            {
                audioSource.PlayOneShot(swordSwingSfx);
                audioSource.PlayOneShot(gruntSfx);
            }
            swordCollider.enabled = true;
        }

        _isAttackingSfx = true;
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
        _isAttackingSfx = false;
    }
}
