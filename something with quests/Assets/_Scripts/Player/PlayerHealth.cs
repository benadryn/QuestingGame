using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

    public delegate void OnPotionUsedHandler(float amount);
    public static OnPotionUsedHandler OnPotionUsed;
    
    [SerializeField] private bool resetHp;
    private const float StartHp = 10.0f;
    private static readonly int IsDamaged = Animator.StringToHash("isDamaged");
    private static readonly int IsDead = Animator.StringToHash("isDead");

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip swordHitSfx;
    [SerializeField] private AudioClip[] gruntHitSfx;
    [SerializeField] private AudioClip playerDeathSfx;

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
        OnPotionUsed += HealPlayer;
    }

    private void OnDisable()
    {
        AddHpOnLevel -= AddToHealthOnLevelGain;
        OnPotionUsed -= HealPlayer;
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
            int randomClip = Random.Range(0, gruntHitSfx.Length);
            playerHealth.ApplyChange(-damage.damageAmount);
            damageEvent.Invoke();
            _animator.SetBool(IsDamaged, true);
            audioSource.PlayOneShot(swordHitSfx);
            healthSlider.value = playerHealth.Value;
            healthText.text = $"{playerHealth.Value}/{maxPlayerHealth.Value}";
            if (playerHealth.Value <= 0)
            {
                playerHealth.Value = 0;
                _animator.SetBool(IsDead, true);
                audioSource.PlayOneShot(playerDeathSfx);
                return;
            }
            audioSource.PlayOneShot(gruntHitSfx[randomClip]);
        }
    }
    
    public void ResetDamaged()
    {
        _animator.SetBool(IsDamaged, false);
    }

    private void HealPlayer(float amount)
    {
        if (playerHealth.Value + amount > maxPlayerHealth.Value)
        {
            playerHealth.Value = maxPlayerHealth.Value;
        }
        else
        {
            playerHealth.Value += amount;
        }
        UpdateHpText();
    }
}
