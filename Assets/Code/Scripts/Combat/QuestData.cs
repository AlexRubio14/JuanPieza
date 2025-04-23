using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{    
    public enum QuestObjective { BATTLE, TRANSPORT, RESCUE, BOARDING };
    public enum QuestClimete { CLEAR, SNOW, STORM };
    
    public string title;
    public string description;
    public QuestObjective questObjective;
    public PlayerShip ship;
    public int rescueCuantity;

    public List<QuestData> questsToComplete;
    public Vector2 positionInMap;
    public bool completed;

    public List<ShipData> enemiesHordes;

    public QuestClimete questClimete;
    public bool hasWhirlwind;
    public bool hasGeyser;
}

[System.Serializable]
public class PlayerShip
{
    public GameObject _ship;
    public List<ResourceQuantity> resourceCuantity;
}
[System.Serializable]
public class ResourceQuantity
{
    public ObjectSO resource;  
    public int quantity;
}
