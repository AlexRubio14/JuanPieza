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
    private List<Vector3> cannonPossiblePosition = new List<Vector3>();

    public List<Vector3> GetCannonPosition()
    {
        return cannonPossiblePosition;
    }
}

public class GenerateEnemyShip : MonoBehaviour
{
    public static GenerateEnemyShip instance;

    [Header("Enemy Ships")]
    [SerializeField] private LayerMask hitLayer;
    private List<EnemyShip> enemyShipInformation;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;
    }

    private void Start()
    {
        enemyShipInformation = new List<EnemyShip>();
    }
    public void GenerateEnemiesShip()
    { 
        enemyShipInformation =((BattleNodeData)MapManager.Instance.GetCurrentLevel()).enemyShipInformation;

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
        Transform cannonPositionsHolder = ship.transform.Find("CannonPositions");

        if (cannonPositionsHolder != null)
        {
            foreach (Transform point in cannonPositionsHolder)
            {
                enemy.GetCannonPosition().Add(point.localPosition);
            }
        }

        foreach (var cannon in enemy.cannonCuantity)
        {
            GameObject newCannon = Instantiate(cannon);
            newCannon.transform.SetParent(ship.transform);

            int randomIndex = Random.Range(0, enemy.GetCannonPosition().Count);
            Vector3 chosenPosition = enemy.GetCannonPosition()[randomIndex];
            enemy.GetCannonPosition().Remove(chosenPosition);

            if (isLeft)
            {
                newCannon.transform.localPosition = new Vector3(chosenPosition.x, chosenPosition.y, chosenPosition.z);
                newCannon.transform.Rotate(0, 90, 0);
            }
            else
            {
                newCannon.transform.localPosition = new Vector3(-chosenPosition.x, chosenPosition.y, chosenPosition.z);
                newCannon.transform.Rotate(0, -90, 0);
            }

            newCannon.GetComponent<EnemyObject>().enemieManager = ship.GetComponent<EnemieManager>();
        }
    }
}
