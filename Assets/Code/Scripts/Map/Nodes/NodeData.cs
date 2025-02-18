using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeData", menuName = "Scriptable Objects/NodeData")]
public class NodeData : ScriptableObject
{
    public enum NodeType { BATTLE, EVENT, SHOP, BOSS, TUTORIAL }
    public NodeType nodeType;
    public NodeData CretaeNode()
    {
        return new NodeData
        {
            name = this.name,
            nodeType = this.nodeType,
        };
    }
}
