using System.Collections.Generic;
using UnityEngine;


public class GenerateEnemyShip : MonoBehaviour
{
    public static GenerateEnemyShip instance;

    private List<EnemyShip> enemyShipInformation = new List<EnemyShip>();

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;
    }

    public void GenerateEnemiesShip()
    { 
        foreach (var enemy in enemyShipInformation)
        {
            GameObject enemyShip = Instantiate(enemy._ship);
            enemyShip.GetComponent<EnemieManager>().SetTotalEnemies(enemy.enemiesCuantity);
            enemyShip.transform.position = enemy.initShipPosition;

            enemyShip.GetComponent<EnemieManager>().GenerateEnemies();
        }
    }
}
