using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "Scriptable Objects/EventData")]
public class EventNode : NodeData
{
    public enum EventType { CHEST, MINIBOSS, UPGRADE }
    public EventType eventType;

    public int difficult;
}
