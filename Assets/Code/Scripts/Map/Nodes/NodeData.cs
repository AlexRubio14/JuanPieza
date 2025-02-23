using UnityEngine;

public class NodeData : ScriptableObject
{
    public enum NodeType { BATTLE, EVENT, SHOP, BOSS, TUTORIAL }
    public NodeType nodeType;
    public bool hasPirateBoarding;

    public NodeData CretaeNode()
    {
        return new NodeData
        {
            name = this.name,
            nodeType = this.nodeType,
        };
    }
}
