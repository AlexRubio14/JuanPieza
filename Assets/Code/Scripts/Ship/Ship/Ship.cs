using System;
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

    protected Animator animator;
    public Action<GameObject> onDamageRecieved;

    public virtual void Start()
    {
        Initialize();
        animator = GetComponent<Animator>();
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
    }

    public virtual void DestroyShip()
    {
        ShipsManager.instance.RemoveEnemyShip(this);
        Destroy(gameObject);
    }
    public void SetCurrentHealth(float amount)
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
        if (currentHealth < 0)
        {
            currentHealth = 0;
            targetHeight = destroyY;
            if (animator)
                animator.SetBool("Dead", true);
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    #endregion

    

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
