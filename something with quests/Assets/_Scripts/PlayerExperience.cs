using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    public static Action<int> xpGain;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private ParticleSystem levelUpParticle;
    private float _level = 1.0f;
    private int _xpToLevel;
    private int _currentXp;


    private void Awake()
    {
        GetXpToLevel();
        UpdateXpText();
    }


    private void OnEnable()
    {
        xpGain += AddXpToCharacter;
    }

    private void OnDisable()
    {
        xpGain -= AddXpToCharacter;
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
            PlayerHealth.AddHpOnLevel?.Invoke();
            _currentXp -= _xpToLevel;
            _level++;
            levelUpParticle.Play();
            GetXpToLevel();
        }
        UpdateXpText();
    }
}
