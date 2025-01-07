using System.Collections.Generic;
using UnityEngine;

public class ShipSceneManager : MonoBehaviour
{
    public static ShipSceneManager Instance { get; private set; }

    private List<InteractableObjectData> objectsToSpawn = new List<InteractableObjectData>();
    
    [System.Serializable]
    public struct InteractableObjectData
    {
        public GameObject prefab; 
        public Vector3 offsetFromShip;
        public Quaternion rotation;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            InstantiateObjects();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            SetObjectsToSpawn();
        }
    }

    public void SetObjectsToSpawn()
    {
        objectsToSpawn.Clear();

        InteractableObjectData interactableObjectData = new InteractableObjectData();
        Ship ship = ShipsManager.instance.playerShip;

        foreach (InteractableObject obj in ship.GetInventory())
        {
            interactableObjectData.prefab = obj.objectSO.prefab;
            interactableObjectData.offsetFromShip = obj.transform.position - ship.transform.position;
            interactableObjectData.rotation = obj.transform.rotation;
            objectsToSpawn.Add(interactableObjectData);
        }
    }
 
    public void InstantiateObjects()
    {
        foreach(InteractableObjectData obj in objectsToSpawn) 
        {
            Instantiate(obj.prefab, obj.offsetFromShip + ShipsManager.instance.playerShip.transform.position, obj.rotation);
        }
    }
}
