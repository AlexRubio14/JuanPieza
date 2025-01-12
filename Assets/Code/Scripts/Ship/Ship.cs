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

    [Header("Weigth")]
    [SerializeField] private float maxWeigth;
    private List<InteractableObject> objects = new List<InteractableObject>();
    private float currentWeight;

    [Header("WeigthDamage")]
    [SerializeField] private float weigthDamage;

    [Header("Timer")]
    [SerializeField] private float damageTime;
    private float currentTime;

    public Action<GameObject> onDamageRecieved;

    [Header("Votation")]
    [SerializeField] private List<Votation> votations;

    [Header("ID")]
    [SerializeField] protected int idShip;

    [Header("Camera Values")]
    [SerializeField] private float newZ;
    [SerializeField] private float newY;

    private Animator animator;

    [SerializeField] private bool isEnemy = true;

    public void Initialize()
    {
        currentHeight = transform.position.y;

        foreach (Votation _votation in votations)
        {
            _votation.gameObject.SetActive(false);
        }

        animator = GetComponent<Animator>();
    }

    public void InitEnemyShip()
    {
        initY = transform.position.y;
        currentHeight = initY;
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        FlotationLerp();
        WeightControl();
    }

    #region Health Flotation
    private void FlotationLerp()
    {
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * heightChangeSpeed);
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
    }
    private void WeightControl()
    {
        if (currentWeight >= maxWeigth)
        {
            currentTime += Time.deltaTime;
            if (currentTime > damageTime)
            {
                SetCurrentHealth(-weigthDamage);
                currentTime = 0;
            }
        }
        else
        {
            currentTime = 0;
        }
    }

    public void DestroyShip()
    {
        ShipsManager.instance.RemoveEnemyShip(this, isEnemy);
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

    #region Votation
    public void StartVotation()
    {
        MapManager.Instance.SetVotations(votations);
    }
    #endregion

    #region Inventory
    public void AddInteractuableObject(InteractableObject interactableObject, bool setParent = true)
    {
        if (objects.Contains(interactableObject))
            return;

        if (interactableObject.objectSO.objectType == ObjectSO.ObjectType.BOX)
            currentWeight += ((Box)interactableObject).GetItemsInBox() * ((Box)interactableObject).GetItemToDrop().weight;

        if (setParent)
            interactableObject.transform.SetParent(transform);
        
        objects.Add(interactableObject);
        currentWeight += interactableObject.objectSO.weight;
    }
    public void RemoveInteractuableObject(InteractableObject interactableObject)
    {
        if (!objects.Contains(interactableObject))
            return;

        currentWeight -= interactableObject.objectSO.weight;
        objects.Remove(interactableObject);
    }
    
    public void AddWeight(float _weight)
    {
        currentWeight += _weight;
    }
    public void RemoveWeight(float _weight)
    {
        currentWeight -= _weight;
    }

    public List<InteractableObject> GetObjectOfType(ObjectSO.ObjectType _type)
    {
        List<InteractableObject> objectList = new List<InteractableObject>();

        foreach (InteractableObject item in objects)
        {
            if(item.objectSO.objectType == _type)
                objectList.Add(item);
        }

        return objectList;
    }
    public bool ItemExist(ObjectSO _object)
    {
        foreach (InteractableObject item in objects)
        {
            if (item.objectSO == _object)
                return true;
            
        }

        return false;
    }
    public List<InteractableObject> GetInventory()
    {
        return objects;
    }
    #endregion

    public bool CheckOverweight()
    {
        return currentWeight >= maxWeigth;
    }

    public float GetInitY()
    {
        return initY;
    }

    public void SetHeightY(float y, float _initY)
    {
        if (targetHeight == 0)
        {
            targetHeight = initY;
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

    public float GetNewZ()
    {
        return newZ;
    }

    public float GetNewY()
    {
        return newY;
    }
}
