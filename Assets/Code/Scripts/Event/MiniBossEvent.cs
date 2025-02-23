using UnityEngine;

public class MiniBossEvent : MonoBehaviour
{
    [SerializeField] private NodeData eventNode;
    void Start()
    {
        MapManager.Instance.ActiveShipEvent(eventNode);
    }
}
