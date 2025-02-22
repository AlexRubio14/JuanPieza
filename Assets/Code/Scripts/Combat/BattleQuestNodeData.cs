using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleQuestNodeData", menuName = "Scriptable Objects/BattleQuestNodeData")]
public class BattleQuestNodeData : ScriptableObject
{ 
    public List<EnemyShip> enemyShipInformation;
}

[System.Serializable]
public class EnemyShip
{
    public enum SpawnShipCondition { HP, INIT, DESTROY }

    public GameObject _ship;
    public int enemiesCuantity;
    public Vector3 initShipPosition;

    public List<GameObject> cannons;

    public SpawnShipCondition spawnShipCondition;
    public float hpPercentage;

    public bool canBoard;
    public List<BoardShip> boardShips;
}

[System.Serializable]
public class BoardShip
{
    public enum SpawnBoardShipCondition { HP, INIT }

    public GameObject _ship;
    public int enemiesCuantity;
    public Vector3 initShipPosition;

    public SpawnBoardShipCondition spawnBoardShipCondition;
    public float hpPercentage;
}
