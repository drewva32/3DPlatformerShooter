using UnityEngine;
using UnityEngine.UI;

public class ChainBladeHealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private ChainBlade chainBlade;

    private void Awake() => chainBlade.OnHealthChanged += UpdateHealthUI;
    private void OnDestroy() => chainBlade.OnHealthChanged -= UpdateHealthUI;


    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }
}