using System;
using System.Collections.Generic;
using UnityEngine;

public class AllyShip : Ship
{
    [Header("Weigth")]
    private List<InteractableObject> objects = new List<InteractableObject>();

    [Header("SpawnPoints")]
    [SerializeField] private List<Transform> playersSpawnPos;

    [Header("Boarding Points")]
    [field: SerializeField] public List<Transform> boardingPoints { get; private set; }

    [Header("Arrive Battle")]
    [field: SerializeField] public float startZPosition { get; private set; }
    [field: SerializeField] public Vector3 cameraInitPosition { get; private set; }
    [SerializeField] private float speed;
    [SerializeField] private float minSpeed;
    public float currentSpeed { get; private set; }
    public float t { get; private set; }
    private float startZ;
    private bool arriving;
    private bool leaving;

    private float recoverHealth;

    [SerializeField] private GameObject behaivour;
    [SerializeField] private ShowMessageRepair[] message;

    [Header("GenerateCannon")]
    [SerializeField] private GameObject cannon;
    [SerializeField] private WoodShelf fishingShelf;

    public override void Start()
    {
        base.Start();
        currentSpeed = speed;
        startZ = transform.position.z;
        AddShipBounds();
    }

    protected override void Update()
    {
        base.Update();

        if (arriving)
            MoveShip(startZ, 0);
        if (leaving)
            MoveShip(0, startZ * -1);
    }

    private void MoveShip(float firstZ, float secondZ)
    {
        t += Time.deltaTime * currentSpeed;
        float newZ = Mathf.Lerp(firstZ, secondZ, t);
        currentSpeed = Mathf.Lerp(speed, minSpeed, t);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
        if (t >= 1)
            arriving = false;
        if (t >= 0.3 && leaving)
        {
            ShipsManager.instance.gameObject.GetComponent<TransitionController>().EndLevelTransition();
            leaving = false;
        }        
    }

    public override void SetCurrentHealth(float amount)
    {
        base.SetCurrentHealth(amount);

        ShipsManager.instance.playerHealthController.SetHealthBar(GetCurrentHealth() / GetMaxHealth());
        
        if(amount < 0)
            recoverHealth += -1 * amount;
        else
            recoverHealth -= amount;
        

    }

    public void SetRecoverHealth(float amount)
    {
        recoverHealth -= amount;
        
        ShipsManager.instance.playerHealthController.SetEaseHealthbar((recoverHealth + GetCurrentHealth()) / GetMaxHealth());
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

    public int GetTotalObjectQuantity(ObjectSO _object)
    {
        int totalObjects = 0;

        foreach (InteractableObject item in objects)
        {
            if (item.objectSO == _object)
                totalObjects++;

        }

        return totalObjects;
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

    public bool GetTotalWeaponsInShip()
    {
        List<InteractableObject> objectList = GetObjectOfType(ObjectSO.ObjectType.WEAPON);
        if (objectList.Count > 1)
            return true;
        return false;
    }

    public void GenerateCannon()
    {
        if (fishingShelf == null && !ShipsManager.instance.playerShip.GetTotalWeaponsInShip())
        {
            Instantiate(cannon, new Vector3(0,5,0), Quaternion.identity).transform.SetParent(this.gameObject.transform);
        }
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

    public void SetArriving(bool state)
    {
        arriving = state;
    }

    public void SetLeaving(bool state)
    {
        leaving = state;
        t = 0;
    }

    public void SetBehaivour(bool state)
    {
        if (behaivour)
            behaivour.SetActive(state);
    }

    public void SetMessage()
    {
        foreach (var _message in message)
            _message.ActiveBoxMessage();
    }
}
