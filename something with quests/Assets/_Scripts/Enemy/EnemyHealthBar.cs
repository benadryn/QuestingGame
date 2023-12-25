using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthUi;
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        healthUi.fillAmount = currentHealth / maxHealth;
    }

    public void ShowHealthBar()
    {
        gameObject.SetActive(true);
    }
    public void HideHealthBar()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
}
