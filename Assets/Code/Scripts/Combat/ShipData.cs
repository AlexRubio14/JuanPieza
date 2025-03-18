using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipData", menuName = "Scriptable Objects/ShipData")]
public class ShipData : ScriptableObject
{
    public enum SpawnShipCondition { INIT, HP, DESTROY }
    public SpawnShipCondition spawnShipCondition;
    [Range(0,1)] public float hpPercentage;

    public List<EnemyShip> enemyShips;
}

[System.Serializable]
public class EnemyShip
{ 
    public GameObject _ship;
    public int enemiesCuantity;
    public Vector3 initShipPosition;
    public List<GameObject> cannons;
    public bool canBoard;
    public List<BoardShip> boardShips;
    public EnemiesStats stats;
}

[System.Serializable]
public class BoardShip
{
    public enum SpawnBoardShipCondition { HP, INIT }

    public GameObject _ship;
    public int enemiesCuantity;

    public SpawnBoardShipCondition spawnBoardShipCondition;
    [Range(0, 1)] public float hpPercentage;
    public float timeToSpawnBoarding;
}
[System.Serializable]
public class EnemiesStats
{
    public float TimeToGetResources;
    public float TimeToRepair;
    public float TimeToInteract;
    public float TimeToShoot;
}
