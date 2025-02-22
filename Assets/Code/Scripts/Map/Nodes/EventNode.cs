using UnityEngine;

public class EventNode : NodeData
{
    public enum EventType { CHEST, MINIBOSS, UPGRADE }
    public EventType eventType;

    public int difficult;
}
