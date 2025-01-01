using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeData", menuName = "Scriptable Objects/NodeData")]
public class NodeData : ScriptableObject
{
    public enum NodeType { BATTLE, EVENT, SHOP, BOSS }

    public NodeType nodeType;
    public int difficult;
    public string sceneName;
    public List<NodeData> children;
    public int nodeHeigth;

    public NodeData CretaeNode()
    {
        return new NodeData
        {
            name = this.name,
            nodeType = this.nodeType,
            nodeHeigth = this.nodeHeigth,
            children = new List<NodeData>()
        };
    }
}
