using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipsManager : MonoBehaviour
{
    public static ShipsManager instance;
    public AllyShip playerShip {  get; private set; }
    public List<EnemyShip> enemiesShipsInformatio { get; private set; }
    public List<Ship> enemiesShips { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;

        enemiesShipsInformatio = new List<EnemyShip>();
        enemiesShips = new List<Ship>();
    }

    private void Start()
    {
        GenerateAllyShip();
        GenerateEnemyShip();
    }

    private void GenerateAllyShip()
    {
        GameObject _ship;

        _ship = Instantiate(NodeManager.instance.questShip.ship._ship);
        _ship.transform.position = new Vector3(0, _ship.GetComponent<AllyShip>().GetInitY(), 0);
        playerShip = _ship.GetComponent<AllyShip>();

        ItemBox[] itemBoxes = _ship.GetComponentsInChildren<ItemBox>();

        foreach (var items in itemBoxes)
        {
            foreach(var resources in NodeManager.instance.questShip.ship.resourceCuantity)
            {
                if(items.GetItemDrop() == resources.resource)
                {
                    items.AddItemInBox(false, resources.quantity);
                }
            }
        }
    }
    private void GenerateEnemyShip()
    {
        foreach (var enemy in NodeManager.instance.battleInformation.enemyShipInformation)
        {
            if(enemy.spawnShipCondition == EnemyShip.SpawnShipCondition.INIT)
            {
                GameObject enemyShip = Instantiate(enemy._ship);
                enemyShip.GetComponent<EnemieManager>().SetTotalEnemies(enemy.enemiesCuantity);
                enemyShip.transform.position = enemy.initShipPosition;
                enemiesShips.Add(enemyShip.GetComponent<Ship>());
                enemyShip.GetComponent<EnemieManager>().GenerateEnemies();

                enemyShip.GetComponent<ShipEnemy>().SetIsArriving(true);
            }

            enemiesShipsInformatio.Add(enemy);
        }  
    }


    public void RemoveEnemyShip(Ship ship)
    {
        enemiesShips.Remove(ship);
    }

    public void RemoveAllyShip()
    {
        AudioManager.instance.StopLoopSound(AudioManager.instance.seagullAs);
        SceneManager.LoadScene("HUB");
    }

}
