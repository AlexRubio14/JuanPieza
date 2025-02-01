using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class EnemyShip
{
    public GameObject _ship;
    public Vector3 initShipPosition;
    public int enemiesCuantity;
    public List<GameObject> cannonCuantity;
    public float cannonOffset;
    public int cannonPossiblePosition;
    public Vector3 initCannonPosition;
}

public class GenerateEnemyShip : MonoBehaviour
{
    public static GenerateEnemyShip instance;

    [Header("Enemy Ships")]
    public List<EnemyShip> enemyShipInformation;

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

            GenerateCannons(enemy.initShipPosition.x > 0, enemy, enemyShip);

            ShipsManager.instance.AddEnemyShip(enemyShip.GetComponent<Ship>());
            enemyShip.GetComponent<EnemieManager>().GenerateEnemies();
        }
    }

    private void GenerateCannons(bool isLeft, EnemyShip enemy, GameObject ship)
    {
        List<float> cannonZPosition = new List<float>();

        for (int i = 0; i < enemy.cannonPossiblePosition; i++)
        {
            cannonZPosition.Add(enemy.initCannonPosition.z + enemy.cannonOffset);
            cannonZPosition.Add(enemy.initCannonPosition.z - enemy.cannonOffset);
            enemy.cannonOffset += enemy.cannonOffset;
        }

        cannonZPosition.Add(enemy.initCannonPosition.z);

        foreach (var cannon in enemy.cannonCuantity)
        {
            GameObject newCannon = Instantiate(cannon);
            newCannon.transform.SetParent(ship.transform);

            float randomZ = cannonZPosition[Random.Range(0, cannonZPosition.Count)];
            cannonZPosition.Remove(randomZ);

            if (isLeft)
            {
                newCannon.transform.localPosition = new Vector3(enemy.initCannonPosition.x, enemy.initCannonPosition.y,randomZ);
                newCannon.transform.Rotate(0, 90, 0);
            }
            else
            {
                newCannon.transform.localPosition = new Vector3(-enemy.initCannonPosition.x, enemy.initCannonPosition.y,randomZ);
                newCannon.transform.Rotate(0, -90, 0);
            }

            newCannon.GetComponent<EnemyObject>().enemieManager = ship.GetComponent<EnemieManager>();
        }


    }
}
