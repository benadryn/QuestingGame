using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static Action AddHpOnLevel;
    
    [SerializeField] private FloatVariable playerHealth;
    [SerializeField] private FloatVariable maxPlayerHealth;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private UnityEvent damageEvent;
    [SerializeField] private float hpPerLevelGain = 2.0f;
    private Animator _animator;
    
    [SerializeField] private bool resetHp;
    private const float StartHp = 10.0f;
    private static readonly int IsDamaged = Animator.StringToHash("isDamaged");
    private static readonly int IsDead = Animator.StringToHash("isDead");

    private void Awake()
    {
        if (resetHp)
        {
            playerHealth.Value = StartHp;
            maxPlayerHealth.Value = StartHp;
        }
        UpdateHpText();

        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        AddHpOnLevel += AddToHealthOnLevelGain;
    }

    private void OnDisable()
    {
        AddHpOnLevel -= AddToHealthOnLevelGain;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnDamage(other);
    }

    private void UpdateHpText()
    {
        healthSlider.maxValue = maxPlayerHealth.Value;
        healthSlider.value = playerHealth.Value;
        healthText.text = $"{playerHealth.Value}/{maxPlayerHealth.Value}";
    }
    
    private void AddToHealthOnLevelGain()
    {
        maxPlayerHealth.Value += hpPerLevelGain;
        playerHealth.Value = maxPlayerHealth.Value;
        UpdateHpText();
    }
    
    private void OnDamage(Collider other)
    {
        DamageDealer damage = other.gameObject.GetComponent<DamageDealer>();

        if (damage != null && playerHealth.Value > 0)
        {
            playerHealth.ApplyChange(-damage.damageAmount);
            damageEvent.Invoke();
            _animator.SetBool(IsDamaged, true);
            if (playerHealth.Value <= 0)
            {
                playerHealth.Value = 0;
                _animator.SetBool(IsDead, true);
            }

            healthSlider.value = playerHealth.Value;
            healthText.text = $"{playerHealth.Value}/{maxPlayerHealth.Value}";
        }
    }
    
    public void ResetDamaged()
    {
        _animator.SetBool(IsDamaged, false);

    }
}
