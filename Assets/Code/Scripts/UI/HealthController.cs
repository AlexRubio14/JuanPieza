using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public static HealthController instance;

    [SerializeField] private Image healthBarImage;
    [SerializeField] private Image easeHealthBarImage;
    [SerializeField, Range(0.05f, 0.5f)]  private float lerpSpeed = 0.05f; 
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    
    public void SetHealthBar(float health)
    {    
        if (healthBarImage != null)
            healthBarImage.fillAmount = health;
        
        if (easeHealthBarImage != null)
            easeHealthBarImage.fillAmount = Mathf.Lerp(easeHealthBarImage.fillAmount, health, lerpSpeed);
    }
}
