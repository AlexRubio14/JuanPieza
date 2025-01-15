using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShipSceneManager : MonoBehaviour
{
    public static ShipSceneManager Instance { get; private set; }

    [SerializeField] private GameObject[] ship;

    private List<InteractableObjectData> objectsToSpawn = new List<InteractableObjectData>();
    private List<Vector3> playersPosition = new List<Vector3>();
    private int shipId;
    private float shipHealth;
    private float shipCurrentY;
    private float shipInitY;
    
    public Dictionary<ObjectSO, int> shipBoxes = new Dictionary<ObjectSO, int>();
    
    [Header("Delete post Alp")]
    [SerializeField] private ObjectSO[] boxesDefaultKey;
    [SerializeField] private int[] boxesDefaultValue;
    
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

    private void Start()
    {
        for (int i = 0; i < boxesDefaultKey.Length; i++)
        {
            shipBoxes.Add(boxesDefaultKey[i], boxesDefaultValue[i]);
        }
    }

    public void SetObjectsToSpawn()
    {
        objectsToSpawn.Clear();

        InteractableObjectData interactableObjectData = new InteractableObjectData();
        Ship ship = ShipsManager.instance.playerShip;

        foreach (InteractableObject obj in ship.GetInventory())
        {
            // Quitar los if despues de la entrega
            if (obj.objectSO.objectType != ObjectSO.ObjectType.BOX)
            {
                interactableObjectData.prefab = obj.objectSO.prefab;
                interactableObjectData.offsetFromShip = obj.transform.position - ship.transform.position;
                interactableObjectData.rotation = obj.transform.rotation;
                objectsToSpawn.Add(interactableObjectData);
            }
            else
            {
                shipBoxes[obj.objectSO] = (obj as Box).GetItemsInBox();
            }
        }
    }

    public void SetShipId(int id, float currentHealth, float currentY)
    {
        shipId = id;
        shipHealth = currentHealth;
        shipCurrentY = currentY;
        shipInitY = ShipsManager.instance.playerShip.GetInitY();
    }

    public void SetPlayerPosition()
    {
        Ship ship = ShipsManager.instance.playerShip;
        foreach (PlayerController player in PlayersManager.instance.ingamePlayers)
        {
            playersPosition.Add(player.gameObject.transform.position - ship.transform.position);
        }
    }

    public List<Vector3> GetPlayersPositions()
    {
        return playersPosition;
    }
 
    public void InstantiateObjects()
    {
        foreach(InteractableObjectData obj in objectsToSpawn) 
        {
            GameObject _object = Instantiate(obj.prefab, obj.offsetFromShip + ShipsManager.instance.playerShip.transform.position, obj.rotation);
            ShipsManager.instance.playerShip.GetComponent<Ship>().AddInteractuableObject(_object.GetComponent<InteractableObject>());
        }
        
    }

    public void InstantiateShip()
    {
        GameObject _ship;
        if (MapManager.Instance.GetCurrentLevel().hasIsland)
            _ship = Instantiate(ship[shipId + 1], new Vector3(0, ship[shipId].GetComponent<Ship>().GetInitY(), 0), Quaternion.identity);
        else
            _ship = Instantiate(ship[shipId], new Vector3(0, ship[shipId].GetComponent<Ship>().GetInitY(), 0), Quaternion.identity);

        _ship.GetComponent<Ship>().SetHealth(shipHealth);
        _ship.GetComponent<Ship>().SetHeightY(shipCurrentY, shipInitY);
        ShipsManager.instance.SetShip(_ship.GetComponent<Ship>());
        SetBoxesItem();
        InstantiateObjects();
    }

    // Borrar despues de la entrega
    public void SetBoxesItem()
    {
        Box[] boxes = ShipsManager.instance.playerShip.GetComponentsInChildren<Box>();
        foreach (Box box in boxes)
        {
            for (int i = 0; i < shipBoxes[box.objectSO]; i++)
            {
                box.AddItemInBox();
            }
        }
    }
    
}
