using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleData", menuName = "Scriptable Objects/BattleData")]
public class BattleNodeData : NodeData
{
    public int levelMoney;
    public int difficult;

    public List<EnemyShip> enemyShipInformation;
}
