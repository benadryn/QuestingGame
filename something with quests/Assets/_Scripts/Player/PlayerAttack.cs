using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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
    private static readonly int IsHeavyAttacking = Animator.StringToHash("isHeavyAttacking");

    private bool _isHoldingAttack;
    private float _attackStartTime;
    [SerializeField] private float strongAttackHoldTime = 0.5f;
    [SerializeField] private FloatVariable swordDamage;

    [SerializeField] private float normalAttackDamage = 1.2f;
    [SerializeField] private float heavyAttackDamageMulti = 1.5f;
    
    [Header("Audio")] 
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip swordSwingSfx;

    [SerializeField] private Collider swordCollider;

    private void Awake()
    {
        _playerAnimator = GetComponent<Animator>();
        _playerControls = new PlayerControls();


        _swordSwing = _playerControls.Player.Attacking;

        _swordSwing.started += ctx =>
        {
            // check if over a quest ui before attacking
            if (!IsPointerOverUiNonAttackable("NonAttackableUi"))
            {
                _attackStartTime = Time.time;
            }
        };

        _swordSwing.canceled += ctx =>
        {
            if (!IsPointerOverUiNonAttackable("NonAttackableUi"))
            {
                if (_isAttacking) return;
                float attackDuration = Time.time - _attackStartTime;

                if (attackDuration >= strongAttackHoldTime)
                {
                    StrongAttack();
                }
                else
                {
                    NormalAttack();
                }
            }


        };
    }


    private void StrongAttack()
    {
        _playerAnimator.SetBool(IsHeavyAttacking, true);
        _playerAnimator.SetBool(IsAttacking, true);
        swordDamage.Value *= heavyAttackDamageMulti;

    }

    private void NormalAttack()
    {
        _playerAnimator.SetBool(IsAttacking, true);
        swordDamage.Value *= normalAttackDamage;

    }
    

    private bool IsPointerOverUiNonAttackable(string tag)
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            foreach (var result in raycastResults)
            {
                if (result.gameObject.CompareTag(tag))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void Start()
    {
         _playerMovement = PlayerMovement.Instance;
         swordHit = GetComponentInChildren<SwordHit>();
         swordCollider = swordHit.GetComponentInChildren<BoxCollider>();

    }

    private void Update()
    {
        if (_isAttacking)
        {
            swordHit.SendHitRaycast(swordDamage.Value);
        }

        if (!swordCollider || !swordHit)
        {
            swordHit = GetComponentInChildren<SwordHit>();
            swordCollider = swordHit.GetComponentInChildren<BoxCollider>();
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
        swordCollider.enabled = true;
        audioSource.PlayOneShot(swordSwingSfx);
        if (_playerAnimator.GetBool(IsRunning))
        {
            _playerMovement.RotateWhenAttacking();
        }

    }
    private void StopAttack()
    {
        if(!swordCollider) return;
        swordCollider.enabled = false;
        _isAttacking = false;
        _playerAnimator.SetBool(IsHeavyAttacking, false);
        _playerAnimator.SetBool(IsAttacking, false);
        swordDamage.Value = normalAttackDamage;
        swordHit.ResetIsHit();
        
    }
}
