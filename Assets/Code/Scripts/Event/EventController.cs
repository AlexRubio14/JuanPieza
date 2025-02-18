using UnityEngine;

public class EventController : MonoBehaviour
{
    [Header("MiniBoss Event")]
    [SerializeField] private NodeData eventNode;

    [Header("Chest Event")]
    [SerializeField] private ChestController chestEvent;

    [Header("Upgrade Event")]
    [SerializeField] private GameObject upgradeEvent;
    void Start()
    {
        switch(((EventNode)MapManager.Instance.GetCurrentLevel()).eventType)
        {
            case EventNode.EventType.CHEST:
                chestEvent.ActiveChest(true);
                break;
            case EventNode.EventType.UPGRADE:
                upgradeEvent.SetActive(true);
                break;
            case EventNode.EventType.MINIBOSS:
                MapManager.Instance.AddNodesDone();
                MapManager.Instance.ActiveShipEvent(eventNode);
                break;
        }
    }
}
