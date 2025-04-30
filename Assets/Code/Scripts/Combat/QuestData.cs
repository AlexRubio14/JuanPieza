using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{    
    public enum QuestObjective { BATTLE, TRANSPORT, RESCUE, BOARDING };
    public enum QuestClimete { CLEAR, SNOW, STORM };
    
    public string title;
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

    public EnemiesStats stats;

    public void SetCompleted(bool _completed)
    {
        completed = _completed;
    }
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

[System.Serializable]
public class EnemiesStats
{
    public float TimeToGetResources;
    public float TimeToRepair;
    public float TimeToInteract;
    public float TimeToShoot;
    public float Speed;
}


