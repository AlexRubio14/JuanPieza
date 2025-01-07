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
    private Dictionary<InteractableObject, int> objects;
    private float currentWeight;

    [Header("WeigthDamage")]
    [SerializeField] private float weigthDamage;

    [Header("Timer")]
    [SerializeField] private float damageTime;
    private float currentTime;

    public Action<GameObject> onDamageRecieved;
    [Header("Votation")]
    [SerializeField] private List<Votation> votations;

    public void Initialize()
    {
        currentHealth = maxHealth;
        initY = transform.position.y;
        targetHeight = initY;
        currentHeight = transform.position.y;

        objects = new Dictionary<InteractableObject, int>();

        foreach (Votation _votation in votations)
        {
            _votation.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    StartVotation();
        //}

        FlotationLerp();
        WeightControl();
    }
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

    public void StartVotation()
    {
        MapManager.Instance.SetVotations(votations);
    }

    public void AddInteractuableObject(InteractableObject interactableObject)
    {
        if (objects.ContainsKey(interactableObject))
        {
            objects[interactableObject]++;
        }
        else
        {
            objects[interactableObject] = 1;
        }
        currentWeight += interactableObject.objectSO.weight;
    }

    public void RemoveInteractuableObject(InteractableObject interactableObject)
    {
        if (objects.ContainsKey(interactableObject))
        {
            if (objects[interactableObject] > 1)
            {
                objects[interactableObject]--;
            }
            else
            {
                objects.Remove(interactableObject);
            }
            currentWeight -= interactableObject.objectSO.weight;
        }
    }

    public bool CheckOverweight()
    {
        return currentWeight >= maxWeigth;
    }
}
