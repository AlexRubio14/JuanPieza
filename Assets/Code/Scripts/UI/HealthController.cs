using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Image easeHealthBarImage;
    [SerializeField, Range(0.05f, 0.5f)] private float lerpSpeed = 0.05f;
    [SerializeField, Range(0.01f, 10.0f)] private float disvanishSpeed = 7.0f;

    private Image canvasImage;
    
    private bool enemyHealthBar;
    private float targetHealth = 100;
    private bool isVanishing;
    private float vanishTimer;

    private void Start()
    {
        canvasImage = GetComponent<Image>();
        StartVanish();
    }

    public void SetHealthBar(float health)
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = health;
        }

        if (easeHealthBarImage != null)
        {
            targetHealth = health;
            
            if (enemyHealthBar)
                StartVanish();
        }
    }

    public void SetEaseHealthbar(float health)
    {
        if (easeHealthBarImage != null)
        {
            easeHealthBarImage.fillAmount = health;
        }
    }

    private void Update()
    {
        if (enemyHealthBar)
        {
            SetEaseHealthbar(Mathf.Lerp(easeHealthBarImage.fillAmount, targetHealth, lerpSpeed));
            
            if (isVanishing)
                VanishHealthBars();
        }
    }

    public void IsEnemyHealth(bool isEnemyHealth)
    {
        enemyHealthBar = isEnemyHealth;
    }

    public void StartVanish()
    {
        isVanishing = true;
        vanishTimer = 0f;
    }

    private void VanishHealthBars()
    {
        vanishTimer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, vanishTimer / disvanishSpeed);

        SetImageAlpha(healthBarImage, alpha);
        SetImageAlpha(easeHealthBarImage, alpha);
        SetImageAlpha(canvasImage, alpha);
        if (vanishTimer >= disvanishSpeed)
        {
            isVanishing = false;
            SetImageAlpha(healthBarImage, 0f);
            SetImageAlpha(easeHealthBarImage, 0f);
            SetImageAlpha(canvasImage, 0f);
        }
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}