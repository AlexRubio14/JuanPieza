using System.Collections.Generic;
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
    private bool shipHasCatapult;

    public Dictionary<ObjectSO, int> shipBoxes = new Dictionary<ObjectSO, int>();
    
    [Header("Delete post Alp")]
    [SerializeField] private ObjectSO[] boxesDefaultKey;
    [SerializeField] private int[] boxesDefaultValue;

    [SerializeField] private float islandArriveDistance;
    
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
        AllyShip ship = ShipsManager.instance.playerShip;

        foreach (InteractableObject obj in ship.GetInventory())
        {
            // Quitar los if despues de la entrega
            if (obj.objectSO.objectType != ObjectSO.ObjectType.BOX && obj.objectSO.objectType != ObjectSO.ObjectType.TOOL)
            {
                interactableObjectData.prefab = obj.objectSO.prefab;
                interactableObjectData.offsetFromShip = obj.transform.position - ship.transform.position;
                interactableObjectData.rotation = obj.transform.rotation;
                objectsToSpawn.Add(interactableObjectData);
            }
            else if(obj.objectSO.objectType == ObjectSO.ObjectType.BOX)
            {
                SaveBoxData(obj.objectSO, obj as Box);
            }
        }
    }

    public void SaveBoxData(ObjectSO objectSo, Box box)
    {
        shipBoxes[objectSo] = box.GetItemsInBox();
    }

    public void SetShipId(int id, float currentHealth, float currentY, bool currentHasCatapult)
    {
        shipId = id;
        shipHealth = currentHealth;
        shipCurrentY = currentY;
        shipHasCatapult = currentHasCatapult;
        shipInitY = ShipsManager.instance.playerShip.GetInitY();
    }
 
    public void InstantiateObjects()
    {
        foreach(InteractableObjectData obj in objectsToSpawn) 
        {
            GameObject _object = Instantiate(obj.prefab, obj.offsetFromShip + ShipsManager.instance.playerShip.transform.position, obj.rotation);
            ShipsManager.instance.playerShip.GetComponent<AllyShip>().AddInteractuableObject(_object.GetComponent<InteractableObject>());
        }
        
    }

    public void InstantiateShip()
    {
        GameObject _ship;
        if (MapManager.Instance.GetCurrentLevel().nodeType == NodeData.NodeType.TUTORIAL)
        {
            _ship = Instantiate(ship[1], new Vector3(0, ship[shipId].GetComponent<Ship>().GetInitY(), 0), Quaternion.identity);
            _ship.GetComponent<ShipCurve>().SetIsMainShip(true);
            _ship.GetComponent<ShipCurve>().ActiveBridge(false);
        }
        else if (MapManager.Instance.GetCurrentLevel().hasIsland)
        {
            _ship = Instantiate(ship[shipId + 1], new Vector3(0, ship[shipId].GetComponent<Ship>().GetInitY(), -islandArriveDistance), Quaternion.identity);
            CameraManager.Instance.SetArriveCamera(true);
            CameraManager.Instance.SetSimpleCamera(false);
            _ship.GetComponent<ShipCurve>().SetIsMainShip(true);
            _ship.GetComponent<ShipCurve>().ActiveBridge(false);
        }
        else
            _ship = Instantiate(ship[shipId], new Vector3(0, ship[shipId].GetComponent<Ship>().GetInitY(), 0), Quaternion.identity);

        ShipsManager.instance.SetShip(_ship.GetComponent<AllyShip>());
        ShipsManager.instance.playerShip.SetHealth(shipHealth);
        ShipsManager.instance.playerShip.SetHeightY(shipCurrentY, shipInitY);
        ShipsManager.instance.playerShip.SetBarrelBox(shipHasCatapult);
        InstantiateObjects();
        SetBoxesItem();
    }

    // Borrar despues de la entrega
    public void SetBoxesItem()
    {
        Box[] boxes = ShipsManager.instance.playerShip.GetComponentsInChildren<Box>();
        foreach (Box box in boxes)
        {
            if(box.GetItemDrop().objectType == ObjectSO.ObjectType.TOOL)
            {
                PutItemsInBox(4, box);
            }
            else
            {
                PutItemsInBox(shipBoxes[box.objectSO], box);
            }
        }
    }

    private void PutItemsInBox(int cuantity, Box box)
    {
        for (int i = 0; i < cuantity; i++)
        {
            box.AddItemInBox(false);
        }
    }
    
}
