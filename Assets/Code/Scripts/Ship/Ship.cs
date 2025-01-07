using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [Header("HealthVariables")]
    [SerializeField] private float maxHealth;
    private float currentHealth;

    [Header("LerpValues")]
    [SerializeField] private float heightChangeSpeed;
    [SerializeField] private float lowerY;
    private float initY;
    private float targetHeight;
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

    private void Start()
    {
        currentHealth = maxHealth;
        initY = transform.position.y;
        targetHeight = initY;
        currentHeight = transform.position.y;

        foreach (Votation _votation in votations)
        {
            _votation.gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.X)) 
        //{
        //    StartVotation();
        //}

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
            //Ense�ar mensaje
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
    public void SetCurrentHealth(float amount)
    {
        currentHealth += amount;
        CheckHealth();
        targetHeight = Mathf.Lerp(lowerY, initY, currentHealth / maxHealth);
    }
    private void CheckHealth()
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
            //Destroy
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
    public void AddInteractuableObject(InteractableObject interactableObject)
    {
        if (objects.Contains(interactableObject))
            return;

        if (interactableObject.objectSO.objectType == ObjectSO.ObjectType.BOX)
        {
            currentWeight += ((Box)interactableObject).GetItemsInBox() * ((Box)interactableObject).GetItemToDrop().weight;
        }

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

    public List<Weapon> GetAllWeapons()
    {
        List<Weapon> weaponList = new List<Weapon>();

        foreach (InteractableObject item in objects)
        {
            if(item is Weapon)
                weaponList.Add((Weapon)item);
        }

        return weaponList;

    }
    #endregion
}
