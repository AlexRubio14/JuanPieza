using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Image easeHealthBarImage;
    [SerializeField, Range(0.05f, 0.5f)]  private float lerpSpeed = 0.05f; 
    
    private float targetHealth;

    public void SetHealthBar(float health)
    {
        if (healthBarImage != null)
            healthBarImage.fillAmount = health;

        if (easeHealthBarImage != null)
        {
            easeHealthBarImage.fillAmount = health;
            targetHealth = health;
        }
            
    }
    
    private void Update()
    {
        if (easeHealthBarImage != null)
            easeHealthBarImage.fillAmount = Mathf.Lerp(easeHealthBarImage.fillAmount, targetHealth, lerpSpeed);
    }
}
