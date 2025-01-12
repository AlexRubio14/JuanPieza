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

    public void SetShipId(int id, float currentHealth, float currentY)
    {
        shipId = id;
        shipHealth = currentHealth;
        shipCurrentY = currentY;
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
        _ship.GetComponent<Ship>().SetHeightY(shipCurrentY);
        ShipsManager.instance.SetShip(_ship.GetComponent<Ship>());
        InstantiateObjects();
    }
}
