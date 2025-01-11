using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    public static ShipsManager instance;

    [field: SerializeField]
    public Ship playerShip {  get; private set; }
    [field: SerializeField]
    public Ship[] enemiesShips { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;
    }

    private void Start()
    {
        ShipSceneManager.Instance.InstantiateShip();
    }

    public void SetShip(Ship ship)
    {
        playerShip = ship;
    }
}
