using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Animator _playerAnimator;
    private PlayerControls _playerControls;
    private InputAction _swordSwing;
    private bool _isAttacking;
    [SerializeField] private SwordHit swordHit;
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    [Header("Audio")] 
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip swordSwingSfx;

    [SerializeField] private Collider swordCollider;

    private void Awake()
    {
        _playerAnimator = GetComponent<Animator>();
        _playerControls = new PlayerControls();
        swordHit.GetComponent<SwordHit>();
        

        _swordSwing = _playerControls.Player.Attacking;

        _swordSwing.performed += ctx =>
        {
            if (ctx.ReadValue<float>() != 0 )
            { 
                swordCollider.enabled = true;
                // if (EventSystem.current.IsPointerOverGameObject()) return;
                _playerAnimator.SetBool(IsAttacking, true);

                if (!_playerAnimator.GetBool(IsRunning))
                {
                    _playerMovement.RotateWhenAttacking();
                }
            }
        };
    }

    private void Start()
    {
         _playerMovement = PlayerMovement.Instance;

    }

    private void Update()
    {
        if (_isAttacking)
        {
            swordHit.SendHitRaycast();
        }
    }

    private void OnEnable()
    {
        
        _swordSwing.Enable();
        
    }

    private void OnDisable()
    {
        _swordSwing.Disable();
    }

    private void StartAttack()
    {
        _isAttacking = true;
        audioSource.PlayOneShot(swordSwingSfx);

    }
    private void StopAttack()
    {
        swordCollider.enabled = false;
        _isAttacking = false;
        _playerAnimator.SetBool(IsAttacking, false);
        swordHit.ResetIsHit();
    }
}
