using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    public static Action<int> XpGain;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private ParticleSystem levelUpParticle;
    private float _level = 1.0f;
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
        _xpToLevel = Mathf.RoundToInt(_level * 100.0f * 1.25f);
    }
    
    private void UpdateXpText()
    {
        xpSlider.maxValue = _xpToLevel;
        xpSlider.value = _currentXp;
        xpText.text = $"{_currentXp}/{_xpToLevel}";
        levelText.text = $"{Mathf.RoundToInt(_level)}";
    }
    
    
    private void AddXpToCharacter(int xpAmount)
    {
        _currentXp += Mathf.RoundToInt(xpAmount / (1 + 0.05f * _level));
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
        _level++;
        levelUpParticle.Play();
        StartCoroutine(StopLevelUpParticle());
        GetXpToLevel();
        audioSource.PlayOneShot(levelUpSfx);
    }


    private IEnumerator StopLevelUpParticle()
    {
        yield return new WaitForSeconds(0.2f);
        levelUpParticle.Stop();
    }
}
