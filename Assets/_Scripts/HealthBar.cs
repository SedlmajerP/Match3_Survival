using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] private Image healthBarSprite;
	[SerializeField] private TMP_Text healthAmount;

	
	public void UpdateHealthBar(float currentHealth, float maxHealth, string target)
	{



		healthBarSprite.fillAmount = currentHealth / maxHealth;
		if(target == "Player")
		{
		healthAmount.text = $"Health {currentHealth}/{maxHealth}";

		}else if(target == "Enemy")
		{
			//healthAmount.text = $"HP: {currentHealth}/{maxHealth}";
		}




	}
}
