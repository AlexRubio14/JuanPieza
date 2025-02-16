using UnityEngine;

[CreateAssetMenu(fileName = "NodeData", menuName = "Scriptable Objects/NodeData")]
public class NodeData : ScriptableObject
{
    public enum NodeType { BATTLE, EVENT, SHOP, BOSS }

    public NodeType nodeType;
    public string sceneName;
    public int levelMoney;
    public bool hasIsland;
    public int difficult;
    public bool hasPirateBoarding;

    public NodeData CretaeNode()
    {
        return new NodeData
        {
            name = this.name,
            sceneName = this.sceneName,
            nodeType = this.nodeType,
        };
    }
}
