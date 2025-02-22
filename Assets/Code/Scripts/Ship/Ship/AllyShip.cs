using System;
using System.Collections.Generic;
using UnityEngine;

public class AllyShip : Ship
{
    [Header("Weigth")]
    private List<InteractableObject> objects = new List<InteractableObject>();

    [Header("SpawnPoints")]
    [SerializeField] private List<Transform> playersSpawnPos;

    protected void InitAllyBoat()
    {

        animator = GetComponent<Animator>();

        if (currentHealth == GetMaxHealth())
            base.Initialize();
    }

    private void Update()
    {
        base.Update();
    }

    public override void DestroyShip()
    {
        ShipsManager.instance.RemoveAllyShip();
        Destroy(gameObject);
    }

    #region Inventory
    public void AddInteractuableObject(InteractableObject interactableObject, bool setParent = true)
    {
        if (objects.Contains(interactableObject))
            return;

        if (setParent)
            interactableObject.transform.SetParent(transform);

        objects.Add(interactableObject);
    }
    public void RemoveInteractuableObject(InteractableObject interactableObject)
    {
        if (!objects.Contains(interactableObject))
            return;

        objects.Remove(interactableObject);
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

    public List<Transform> GetSpawnPoints()
    {
        return playersSpawnPos;
    }
}
