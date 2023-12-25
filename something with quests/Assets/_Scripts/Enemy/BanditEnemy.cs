using System;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Empty,
    Bandit,
    BanditLeader,
}

public class BanditEnemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private int xpToGive = 10;
    [SerializeField] private float respawnTime;
    [SerializeField] private GameObject prefab;
    [SerializeField]private EnemyHealthBar enemyHealthBar;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private float _currentHealth;
    private Animator _animator;
    private Collider _collider;
    private NavMeshAgent _navMeshAgent;
    
    private EnemyRespawn _enemyRespawn;
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private static readonly int Damaged = Animator.StringToHash("Damaged");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        var transform1 = transform;
        _startPosition = transform1.position;
        _startRotation = transform1.rotation;
        _navMeshAgent.enabled = true;
        enemyHealthBar.ShowHealthBar();
    }

    private void Start()
    {
        _enemyRespawn = EnemyRespawn.Instance;
        _currentHealth = maxHealth;
        _collider.enabled = true;
        enemyHealthBar.UpdateHealthBar(maxHealth, _currentHealth);

    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        _animator.SetBool(Damaged, true);
        enemyHealthBar.UpdateHealthBar(maxHealth, _currentHealth);
        if (_currentHealth <= 0)
        {
            Die();
            enemyHealthBar.HideHealthBar();
        }
    }
    private void Die()
    {
        _animator.SetBool(IsDead, true);
        _collider.enabled = false;
        _navMeshAgent.enabled = false;
        QuestManager.instance.AdvanceKillQuest(GetQuestId(), enemyType);
        PlayerExperience.xpGain?.Invoke(xpToGive);
        StartCoroutine(_enemyRespawn.Respawn(respawnTime, prefab, _startPosition, _startRotation, gameObject));
        
    }

    public void ResetDamaged()
    {
        _animator.SetBool(Damaged, false);

    }
    
    
    private string GetQuestId()
    {
        return gameObject.tag;
    }
}
