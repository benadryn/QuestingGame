using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    [Header("Components")]
    private CharacterController _characterController;
    private Animator _characterAnimator;
    [SerializeField] private Camera playerCamera;

    [Header("Movement Parameters")]  
    private float _originalSpeed;
    private float _slowSpeed = 2.0f;
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float rotationSpeed = 5.0f;
    
    [Header("Audio")]
    [SerializeField] private AudioClip runningSfx;

    [SerializeField] private AudioClip[] rollGruntSfx;

    [SerializeField] private AudioSource audioSource;
    private bool _runAudioPlaying;

    private Vector2 _currentMovement;
    private bool _isGrounded;
    private bool _isMoving;
    private bool _isAttacking;
    private bool _isHeavyAttacking;
    private const float Gravity = -9.81f;
    private Vector3 _direction;
    private float _velocity;
    private bool _isRolling;

    private PlayerControls _playerControls;
    private InputAction _moveAction;
    private InputAction _rollAction;

    private static readonly int RunBlend = Animator.StringToHash("RunBlend");
    private static readonly int Roll = Animator.StringToHash("Roll");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    private static readonly int IsDamaged = Animator.StringToHash("isDamaged");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private static readonly int IsHeavyAttacking = Animator.StringToHash("isHeavyAttacking");

    private void Awake()
    {
        InitializeSingleton();
        InitializeComponents();
        InitializeInputActions();
    }

    private void Start()
    {
        _originalSpeed = playerSpeed;
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeComponents()
    {
        _characterController = GetComponent<CharacterController>();
        _characterAnimator = GetComponent<Animator>();
    }

    private void InitializeInputActions()
    {
        _playerControls = new PlayerControls();
        _moveAction = _playerControls.Player.Movement;
        _rollAction = _playerControls.Player.Roll;

        _moveAction.performed += OnMovePerformed;
        _moveAction.canceled += OnMoveCanceled;
        
        _rollAction.performed += OnRollPerformed;

    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        _currentMovement = ctx.ReadValue<Vector2>();
        _isMoving = _currentMovement.x != 0 || _currentMovement.y != 0;
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        _isMoving = false;
        _runAudioPlaying = false;
        audioSource.Stop();
    }

    private void OnRollPerformed(InputAction.CallbackContext ctx)
    {
        if (_isMoving && !_isRolling && !_isAttacking)
        {
            _isRolling = true;
            _characterAnimator.SetBool(Roll, true);
            var randomClip = UnityEngine.Random.Range(0, rollGruntSfx.Length);
            audioSource.PlayOneShot(rollGruntSfx[randomClip]);
        }        
    }


    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        CheckSpeedAnim();
        _characterAnimator.SetBool(IsRunning, _isMoving);
        _isAttacking = _characterAnimator.GetBool(IsAttacking);
        _isHeavyAttacking = _characterAnimator.GetBool(IsHeavyAttacking);
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        HandleMovement();
        HandleDamaged();
    }

    private void HandleMovement()
    {
        
        bool isDamaged = _characterAnimator.GetBool(IsDamaged);
        if (!_characterAnimator.GetBool(IsDead))
        {
            
            if (_isMoving)
            {
                playerSpeed = _isHeavyAttacking || _isAttacking || isDamaged ? _slowSpeed : _originalSpeed;
                var transform1 = playerCamera.transform;
                Vector3 cameraForward = transform1.forward;
                Vector3 cameraRight = transform1.right;
                cameraForward.y = 0;
                cameraRight.y = 0;

                cameraForward.Normalize();
                cameraRight.Normalize();
                _direction = _currentMovement.y * cameraForward + _currentMovement.x * cameraRight;
                _characterController.Move(_direction.normalized * (playerSpeed * Time.deltaTime));
                
                if (!_runAudioPlaying)
                {
                    audioSource.PlayOneShot(runningSfx);
                    _runAudioPlaying = true;
                }
                HandleRotation();
            }

            if (!_isMoving && _isAttacking)
            {
                _characterController.Move(new Vector3(0, _direction.y, 0));
            }

            if (!_isMoving)
            {
                StopRollAnim();
            }
        }
    }
    
    
    private void HandleDamaged()
    {
        if (!_characterAnimator.GetBool(IsDamaged)) return;
        _characterAnimator.SetBool(Roll, false);
        _characterAnimator.SetBool(IsAttacking, false);
    }
    private void HandleRotation()
    {
        if (_characterAnimator.GetBool(IsAttacking))
        {
            RotateWhenAttacking();
        }
        else
        {
            RotateNormal();
        }
    }

    private void RotateNormal()
    {
        float targetAngle = Mathf.Atan2(_currentMovement.x, _currentMovement.y) * Mathf.Rad2Deg;
        float newAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle + playerCamera.transform.eulerAngles.y, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Euler(0.0f, newAngle, 0.0f);
    }

    public void RotateWhenAttacking()
    {
        Quaternion targetRotation = playerCamera.transform.rotation;
        targetRotation.eulerAngles = new Vector3(0, targetRotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void CheckSpeedAnim()
    {
        if (_isMoving)
        {
            float moveSpeed = Mathf.Abs(_currentMovement.x) + Mathf.Abs(_currentMovement.y);
            _characterAnimator.SetFloat(RunBlend, moveSpeed);
        }
    }

    public void StopRollAnim()
    {
        _characterAnimator.SetBool(Roll, false);
        _isRolling = false;
    }

    private void ApplyGravity()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.0f);
        if (_isGrounded && _velocity < 0.0f)
        {
            _velocity = -0.1f;
        }
        else
        {
            _velocity += Gravity * Time.deltaTime;
        }

        _direction.y = _velocity;
        _characterController.Move(new Vector3(0, _direction.y, 0));
    }
}
