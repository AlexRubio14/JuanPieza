using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipsManager : MonoBehaviour
{
    public static ShipsManager instance;
    public AllyShip playerShip {  get; private set; }
    public List<ShipData> enemiesHordes { get; private set; }
    public List<Ship> enemiesShips { get; private set; }

    private ShipData.SpawnShipCondition condition;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;

        enemiesShips = new List<Ship>();
    }

    private void Start()
    {
        GenerateAllyShip();
        enemiesHordes = new List<ShipData>(NodeManager.instance.battleInformation.enemiesHordes);        
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
        foreach (var enemy in enemiesHordes[0].enemyShips)
        {
            GameObject enemyShip = Instantiate(enemy._ship);
            enemyShip.GetComponent<EnemieManager>().SetTotalEnemies(enemy.enemiesCuantity);
            enemyShip.transform.position = enemy.initShipPosition;
            enemiesShips.Add(enemyShip.GetComponent<Ship>());
            enemyShip.GetComponent<EnemieManager>().GenerateEnemies();
            GenerateCannons(enemy, enemyShip);
            enemyShip.GetComponent<ShipEnemy>().SetIsArriving(true);
        }

        if(enemiesHordes.Count > 1) 
            condition = enemiesHordes[1].spawnShipCondition;
    }

    private void GenerateCannons(EnemyShip enemy, GameObject ship)
    {
        Transform positionsFather = ship.transform.Find("CannonPositions");
        List<Vector3> cannonsPosition = new List<Vector3>();

        foreach (Transform position in positionsFather)
        {
            cannonsPosition.Add(position.localPosition);
        }

        foreach (var cannons in enemy.cannons)
        {
            GameObject cannon = Instantiate(cannons);
            cannon.transform.SetParent(ship.transform, true);
            cannon.GetComponent<EnemyObject>().enemieManager = ship.GetComponent<EnemieManager>();
            if(ship.transform.position.x < 0)
            {
                cannon.transform.localPosition = new Vector3(-cannonsPosition[0].x, cannonsPosition[0].y, cannonsPosition[0].z);
                cannon.transform.Rotate(0, -90, 0);
            }
            else
            {
                cannon.transform.localPosition = cannonsPosition[0];
                cannon.transform.Rotate(0, 90, 0);
            }

            cannonsPosition.Remove(cannonsPosition[0]);

        }
    }

    public void RemoveEnemyShip(Ship ship)
    {
        enemiesShips.Remove(ship);

        if (condition != ShipData.SpawnShipCondition.DESTROY || enemiesHordes.Count <= 1)
            return;

        if (enemiesShips.Count == 0)
        {
            enemiesHordes.Remove(enemiesHordes[0]);
            GenerateEnemyShip();
        }
    }

    public void CheckLastEnemyShipHP()
    {
        if (condition != ShipData.SpawnShipCondition.HP || enemiesHordes.Count <= 1)
            return;

        Ship lastShip = enemiesShips[enemiesShips.Count - 1];
        if ((lastShip.GetCurrentHealth() / lastShip.GetMaxHealth()) < enemiesHordes[1].hpPercentage)
        {
            enemiesHordes.Remove(enemiesHordes[0]);
            GenerateEnemyShip();
        }
    }

    public void RemoveAllyShip()
    {
        AudioManager.instance.StopLoopSound(AudioManager.instance.seagullAs);
        SceneManager.LoadScene("HUB");
    }

}
