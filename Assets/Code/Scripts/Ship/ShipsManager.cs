using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipsManager : MonoBehaviour
{
    public static ShipsManager instance;

    [field: SerializeField]
    public Ship playerShip {  get; private set; }
    [field: SerializeField]
    public List<Ship> enemiesShips { get; private set; }

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

    public void RemoveEnemyShip(Ship ship, bool isEnemy)
    {
        if(isEnemy)
        {
            enemiesShips.Remove(ship);
            StartVotation();
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void StartVotation()
    {
        if (enemiesShips.Count == 0)
            playerShip.StartVotation();
    }
}
