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


    [Space, Header("Inventory"), SerializeField]
    private Vector3 checkBoxPosition;
    [SerializeField]
    private Vector3 checkBoxSize;
    [SerializeField]
    private LayerMask objectLayer;

    [Header("Weigth")]
    [SerializeField] private float maxWeigth;
    private List<InteractableObject> objects;
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

        objects = new List<InteractableObject>();

        foreach (Votation _votation in votations)
        {
            _votation.gameObject.SetActive(false);
        }

        CheckItemsInShip();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)) 
        {
            StartVotation();
        }

        FlotationLerp();
        WeightControl();

        if (Input.GetKeyDown(KeyCode.M))
        {
            foreach (InteractableObject item in objects)
            {
                Debug.Log("Hay " + item.name);
            }
        }
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
        ChechHealth();
        targetHeight = Mathf.Lerp(lowerY, initY, currentHealth / maxHealth);
    }
    private void ChechHealth()
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
    private void CheckItemsInShip()
    {
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + checkBoxPosition, checkBoxSize, transform.forward, Quaternion.identity, 1, objectLayer);

        foreach (RaycastHit hit in hits)
            AddInteractuableObject(hit.collider.GetComponent<InteractableObject>());

    }
    public void AddInteractuableObject(InteractableObject interactableObject)
    {
        if (objects.Contains(interactableObject))
            return;

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
    #endregion


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + checkBoxPosition, checkBoxSize);
    }
}
