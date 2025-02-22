using System;
using System.Collections.Generic;
using UnityEngine;

public class AllyShip : Ship
{
    [Header("Weigth")]
    [SerializeField] private float maxWeigth;
    private List<InteractableObject> objects = new List<InteractableObject>();
    private float currentWeight;

    [Header("Timer")]
    [SerializeField] private float damageTime;
    private float currentTime;

    [Header("WeigthDamage")]
    [SerializeField] private float weigthDamage;

    [Header("SpawnPoints")]
    [SerializeField] private List<Transform> playersSpawnPos;

    [Header("Votation")]
    [SerializeField] private List<Votation> votations;

    [Header("ID")]
    [SerializeField] protected int idShip;

    [Header("Camera Values")]
    [SerializeField] private float newZ;
    [SerializeField] private float newY;

    [Header("Boarding Points")]
    [SerializeField] private List<Vector3> boardingPoints;

    [SerializeField] private GameObject barrelBox;
    [SerializeField] protected bool isBarrelBoxActive;
    protected void InitAllyBoat()
    {
        foreach (Votation _votation in votations)
        {
            _votation.gameObject.SetActive(false);
        }

        if (barrelBox != null)
            barrelBox.SetActive(isBarrelBoxActive);

        animator = GetComponent<Animator>();

        if (currentHealth == GetMaxHealth())
            base.Initialize();
    }

    private void Update()
    {
        if (barrelBox != null)
            barrelBox.SetActive(isBarrelBoxActive);

        WeightControl();
        base.Update();
    }

    public override void DestroyShip()
    {
        ShipsManager.instance.RemoveAllyShip();
        Destroy(gameObject);
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

    public void StartVotation()
    {
        MapManager.Instance.SetVotations(votations);
    }

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
        VotationCanvasManager.Instance.SetWeightText(currentWeight, maxWeigth);
    }
    public void RemoveInteractuableObject(InteractableObject interactableObject)
    {
        if (!objects.Contains(interactableObject))
            return;

        currentWeight -= interactableObject.objectSO.weight;
        VotationCanvasManager.Instance.SetWeightText(currentWeight, maxWeigth);
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
            if (item.objectSO.objectType == _type)
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
    public Box GetObjectBoxByObject(ObjectSO _object)
    {
        List<InteractableObject> objectList = ShipsManager.instance.playerShip.GetObjectOfType(ObjectSO.ObjectType.BOX);

        foreach (InteractableObject item in objectList)
        {
            Box currentBox = item as Box;
            if (currentBox.GetItemToDrop() == _object)
                return currentBox;
        }

        return null;
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

    public float GetNewZ()
    {
        return newZ;
    }

    public float GetNewY()
    {
        return newY;
    }

    public List<Transform> GetSpawnPoints()
    {
        return playersSpawnPos;
    }

    public void SetBarrelBox(bool value)
    {
        isBarrelBoxActive = value;
    }


}
