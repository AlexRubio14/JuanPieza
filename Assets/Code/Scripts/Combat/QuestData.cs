using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    public enum QuestType { MAIN, SECUNDARY, TUTORIAL };
    public enum QuestObjective { BATTLE, TRANSPORT, RESCUE };

    public QuestType questType;
    public string description;
    public QuestObjective questObjective;
    public PlayerShip ship;
    public int questReward;
}

[System.Serializable]
public class PlayerShip
{
    public GameObject _ship;
    public List<ResourceQuantity> resourceCuantity;
    public Sprite shipImage;
}
[System.Serializable]
public class ResourceQuantity
{
    public ObjectSO resource;  
    public int quantity;
}
