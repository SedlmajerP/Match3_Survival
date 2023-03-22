using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarSprite;

    
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        Debug.Log("updaeHelath bar");
        healthBarSprite.fillAmount = currentHealth / maxHealth;
    }
}
