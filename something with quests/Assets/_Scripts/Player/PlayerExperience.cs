using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    public static Action<int> XpGain;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private ParticleSystem levelUpParticle;
    [SerializeField] private FloatVariable level;
    private int _xpToLevel;
    private int _currentXp;

    [Header("Audio")] 
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip levelUpSfx;

    private void Awake()
    {
        GetXpToLevel();
        UpdateXpText();
    }


    private void OnEnable()
    {
        XpGain += AddXpToCharacter;
    }
    
    private void OnDisable()
    {
        XpGain -= AddXpToCharacter;
    }

    private void GetXpToLevel()
    {
        _xpToLevel = Mathf.RoundToInt(level.Value * 100.0f * 1.25f);
    }
    
    private void UpdateXpText()
    {
        xpSlider.maxValue = _xpToLevel;
        xpSlider.value = _currentXp;
        xpText.text = $"{_currentXp}/{_xpToLevel}";
        levelText.text = $"{Mathf.RoundToInt(level.Value)}";
    }
    
    
    private void AddXpToCharacter(int xpAmount)
    {
        _currentXp += Mathf.RoundToInt(xpAmount / (1 + 0.05f * level.Value));
        if (_currentXp >= _xpToLevel)
        {
            LevelUp();
        }
        UpdateXpText();
    }

    private void LevelUp()
    {
        PlayerHealth.AddHpOnLevel?.Invoke();
        _currentXp -= _xpToLevel;
        level.Value++;
        GetXpToLevel();
        if (_currentXp > _xpToLevel)
        {
            LevelUp();
            return;
        }
        levelUpParticle.Play();
        StartCoroutine(StopLevelUpParticle());
        audioSource.PlayOneShot(levelUpSfx);
    }


    private IEnumerator StopLevelUpParticle()
    {
        yield return new WaitForSeconds(0.2f);
        levelUpParticle.Stop();
    }
}
