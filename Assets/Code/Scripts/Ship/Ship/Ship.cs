using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [Header("HealthVariables")]
    [SerializeField] private float maxHealth;
    protected float currentHealth;

    [Header("LerpValues")]
    [SerializeField] private float heightChangeSpeed;
    [SerializeField] private float lowerY;
    [SerializeField] private float destroyY;
    [SerializeField] private float initY;
    protected float targetHeight;
    private float currentHeight;

    public Action<GameObject> onDamageRecieved;

    [Space, SerializeField]
    private Collider[] shipCameraBounds;

    [SerializeField] private AudioClip boatDestroyed;
    [SerializeField] private List<ParticleSystem> destructionParticles;
    [SerializeField] float delayBetweenGroups = 0.5f;
    [SerializeField] int particlesPerGroup;
    protected bool destroyed;


    public virtual void Start()
    {
        Initialize();
        destroyed = false;
    }

    protected void AddShipBounds()
    {
        CameraController cam = Camera.main.GetComponent<CameraController>();
        foreach (Collider coll in shipCameraBounds)
            cam.AddBounds(coll.gameObject);
    }


    public void Initialize()
    {
        currentHeight = transform.position.y;
        initY = transform.position.y;
        currentHeight = initY;
        currentHealth = maxHealth;

        targetHeight = initY;
    }

    protected virtual void Update()
    {
        FlotationLerp();
    }

    #region Health Flotation
    private void FlotationLerp()
    {
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * heightChangeSpeed);
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
        if (destroyed && currentHeight - 0.5 <= destroyY)
            DestroyShip();
    }

    public virtual void DestroyShip()
    {
        ShipsManager.instance.RemoveEnemyShip(this);
        Destroy(gameObject);
    }
    virtual public void SetCurrentHealth(float amount)
    {
        currentHealth += amount;
        targetHeight = Mathf.Lerp(lowerY, initY, currentHealth / maxHealth);
        CheckHealth();
    }

    public void SetMaxHealth()
    {
        currentHealth = maxHealth;
    }
    private void CheckHealth()
    {
        if (destroyed)
            return;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            destroyed = true;
            StartCoroutine(ExplodeShipRecursive(0));
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    #endregion

    private IEnumerator ExplodeShipRecursive(int startIndex)
    {
        int remaining = destructionParticles.Count - startIndex;

        if (remaining <= 3)
            targetHeight = destroyY;
        else if(remaining <= 0)
            yield break;

        int currentGroup = Mathf.Min(particlesPerGroup, remaining);

        for (int i = 0; i < currentGroup; i++)
        {
            destructionParticles[startIndex + i].gameObject.SetActive(true);
            destructionParticles[startIndex + i].Play();
            AudioManager.instance.Play2dOneShotSound(boatDestroyed, "SFX", 1, 0.8f, 1.2f);
        }

        yield return new WaitForSeconds(delayBetweenGroups);

        StartCoroutine(ExplodeShipRecursive(startIndex + currentGroup));
    }

    public float GetInitY()
    {
        return initY;
    }

    public void SetHeightY(float y, float _initY)
    {
        if (targetHeight == 0)
        {
            SetDeafultTargetHeight();
            return;
        }

        targetHeight = y;
        initY = _initY;
    }


    public void SetHealth(float health)
    {
        currentHealth = health;
        if (currentHealth == 0)
            currentHealth = maxHealth;
    }

   

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetDeafultTargetHeight()
    {
        targetHeight = initY;
    }

    public void SetCurrentHealht(float health)
    {
        currentHealth = health;
    }
}
