using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField, Range(0.05f, 0.5f)] private float lerpSpeed = 0.05f;
    [SerializeField, Range(0.01f, 10.0f)] private float disvanishSpeed = 7.0f;

    private bool enemyHealthBar;
    private bool isVanishing;
    private float vanishTimer;

    private void Start()
    {
        StartVanish();
    }

    public void SetHealthBar(float health)
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = health;
            StartVanish();
        }
    }

    private void Update()
    {
        if (enemyHealthBar)
        {
            if (isVanishing)
                VanishHealthBars();
        }
    }

    public void IsEnemyHealth(bool isEnemyHealth)
    {
        enemyHealthBar = isEnemyHealth;
    }

    private void StartVanish()
    {
        isVanishing = true;
        vanishTimer = 0f;
    }

    private void VanishHealthBars()
    {
        vanishTimer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, vanishTimer / disvanishSpeed);

        SetAlphaRecursively(transform, alpha);

        if (vanishTimer >= disvanishSpeed)
        {
            isVanishing = false;

            SetAlphaRecursively(transform, 0f);
        }
    }
    private void SetAlphaRecursively(Transform parent, float alpha)
    {
        foreach (Transform child in parent)
        {
            Image childImage = child.GetComponent<Image>();
            if (childImage != null)
            {
                SetImageAlpha(childImage, alpha);
            }

            SetAlphaRecursively(child, alpha);
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