using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{

    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI currentHealthField;
    [SerializeField] private Ratchet playerCharacter;

    private void Start() => playerCharacter.OnHealthChanged += UpdateHealthUI;
    private void OnDisable() => playerCharacter.OnHealthChanged -= UpdateHealthUI;

    
    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
        if (!currentHealthField) 
            return;
        int roundedCurrentHealth = Mathf.CeilToInt(currentHealth);
        currentHealthField.text = roundedCurrentHealth.ToString();

    }
}