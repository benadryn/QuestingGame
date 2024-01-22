using System;
using UnityEngine;
using UnityEngine.AI;

public class BanditBehaviour : MonoBehaviour
{
    [SerializeField] private float playerDetectionRadius = 10.0f;
    [SerializeField] private float navMeshStoppingDistance = 2.2f;
    private Vector3 _originalEnemyPosition;
    private Quaternion _originalEnemyRotation;
    // instance
    private PlayerMovement _playerMovement;
    
    private PlayerMovement _target;
    private NavMeshAgent _navMeshAgent;
    private bool _atStartPos = true;
    private bool _isDead;
    // private bool _isTooFarFromSpawn;
    private Animator _animator;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip spottedPlayerSfx;
     
    private static readonly int IsStill = Animator.StringToHash("IsStill");
    private static readonly int IsFollowing = Animator.StringToHash("IsFollowing");
    private static readonly int IsReturning = Animator.StringToHash("IsReturning");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private static readonly int Damaged = Animator.StringToHash("Damaged");



    private void Awake()
    {
        var transform1 = transform;
        _originalEnemyPosition = transform1.position;
        _originalEnemyRotation = transform1.rotation;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _playerMovement = PlayerMovement.Instance;
    }

    void Update()
    {
        _target = CheckPlayerDistance();
        if (!_target && _navMeshAgent.isActiveAndEnabled)
        {
            ReturnToOriginalPosition();   
        }
        else
        {
            if (!_animator.GetBool(IsDead))
            {
                FollowPlayer();
            }
            else
            {
                StopFollowingPlayer();
            }
        }
    }

    private void FollowPlayer()
    {
        if (_atStartPos)
        {
            audioSource.PlayOneShot(spottedPlayerSfx);
        }
        _animator.SetBool(IsStill, false);
        _animator.SetBool(IsFollowing, true);
        _animator.SetBool(IsReturning, false);
        _atStartPos = false;
        Vector3 targetPosition = _target.transform.position;
        _navMeshAgent.stoppingDistance = navMeshStoppingDistance;
        _navMeshAgent.SetDestination(targetPosition);
        if ((targetPosition - transform.position).magnitude <= _navMeshAgent.stoppingDistance && !_animator.GetBool(Damaged))
        {
            AttackPlayer();
            StopMovingWhileAttacking();

        }
        else
        {
            StopAttacking();
        }
    }
    private void StopFollowingPlayer()
    {
        _animator.SetBool(IsStill, true);
        _animator.SetBool(IsFollowing, false);
        _animator.SetBool(IsAttacking, false);
        if (_navMeshAgent.isActiveAndEnabled)
        {
            _navMeshAgent.SetDestination(transform.position);
        }
    }
    private void AttackPlayer()
    {
        _animator.SetBool(IsAttacking, true);
        _animator.SetBool(IsStill, true);
        _animator.SetBool(IsFollowing, false);
        RotateTowardsPlayer();
    }

    private void StopAttacking()
    {
        _animator.SetBool(IsAttacking, false);
        _animator.SetBool(IsStill, false);
        _animator.SetBool(IsFollowing, true);

    }

    private void StopMovingWhileAttacking()
    {
        _navMeshAgent.isStopped = true;
    }

    public void StartMovingAfterAttack()
    {
        if(_animator.GetBool(IsDead))return;
        _navMeshAgent.isStopped = false;
    }
  

    private void RotateTowardsPlayer()
    {
        Vector3 rotation = Quaternion.LookRotation(_target.transform.position - transform.position).eulerAngles;
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = Quaternion.Euler(rotation);

    }

    private PlayerMovement CheckPlayerDistance()
    {
        Vector3 enemyPosition = transform.position;
        Vector3 toPlayer = _playerMovement.transform.position - enemyPosition;
        toPlayer.y = 0;

        if (toPlayer.magnitude <= playerDetectionRadius)
        {
            return _playerMovement;
        }
        else
        {
            return null;
        }
    }

    //TODO - Reset position when too far from start pos
    // private float DistanceFromStartPosition()
    // {
    //     Vector3 toStart = transform.position - _originalEnemyPosition;
    //     return toStart.magnitude;
    // }
    
    private void ReturnToOriginalPosition()
    {
        _navMeshAgent.stoppingDistance = 0;
        _navMeshAgent.SetDestination(_originalEnemyPosition);
        _animator.SetBool(IsFollowing, false);
        _animator.SetBool(IsReturning, true);
        if (Math.Abs(transform.position.x - _originalEnemyPosition.x) < 0.1 && !_atStartPos)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _originalEnemyRotation, 3.0f);
            if (transform.rotation == _originalEnemyRotation)
            {
                _atStartPos = true;
                _animator.SetBool(IsReturning, false);
                _animator.SetBool(IsStill, true);
            }
        }
    }
}
