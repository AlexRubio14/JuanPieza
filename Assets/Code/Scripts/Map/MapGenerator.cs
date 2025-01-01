using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Parameters")]
    [SerializeField] private int mapHeight; 
    private int currentHeight;

    [Header("Node Data")]
    [SerializeField] private List<NodeData> allNodes;
    [SerializeField] private NodeData startNode;
    [SerializeField] private NodeData boss;
    private Dictionary<NodeData.NodeType, List<NodeData>> nodes;
    private Dictionary<int, List<NodeData>> map;

    private void Start()
    {
        nodes = new Dictionary<NodeData.NodeType, List<NodeData>>(); 
        map = new Dictionary<int, List<NodeData>>(); 

        currentHeight = 0; 

        GenerateMap(); 
    }

    private void GenerateMap()
    {
        nodes = OrderNodes();

        if (!map.ContainsKey(currentHeight))
        {
            map[currentHeight] = new List<NodeData>() { startNode };
        }

        foreach (NodeData node in map[currentHeight])
        {
            node.nodeHeigth = currentHeight;
        }

        SetRandomChildrens(startNode, currentHeight);

        PrintMap();
    }

    private void SetRandomChildrens(NodeData node, int nodeHeight)
    {
        nodeHeight++;

        if (nodeHeight == mapHeight)
        {
            if (!map.ContainsKey(nodeHeight))
            {
                map[nodeHeight] = new List<NodeData>();
            }

            boss.nodeHeigth = nodeHeight;
            map[nodeHeight].Add(boss);

            node.children = new List<NodeData> { boss };

            return;
        }

        List<NodeData> children = new List<NodeData>();

        for (int i = 0; i < 2; i++)
        {
            NodeData.NodeType childType = GetNodeTypeBasedOnProbability();
            NodeData originalNode = nodes[childType].OrderBy(x => Random.value).FirstOrDefault();
            NodeData selectedNode = originalNode?.CretaeNode();

            if (selectedNode != null)
            {
                selectedNode.nodeHeigth = nodeHeight;
                children.Add(selectedNode);
            }
        }

        node.children = children;

        if (!map.ContainsKey(nodeHeight))
        {
            map[nodeHeight] = new List<NodeData>();
        }

        foreach (var child in children)
        {
            map[nodeHeight].Add(child);
        }

        foreach (NodeData child in children)
        {
            SetRandomChildrens(child, child.nodeHeigth);
        }
    }

    private NodeData.NodeType GetNodeTypeBasedOnProbability()
    {
        float randomChance = Random.value; 

        if (randomChance <= 0.80f)
        {
            return NodeData.NodeType.BATTLE;
        }
        else if (randomChance <= 0.95f)
        {
            return NodeData.NodeType.SHOP;
        }
        else
        {
            return NodeData.NodeType.EVENT;
        }
    }

    private Dictionary<NodeData.NodeType, List<NodeData>> OrderNodes()
    {
        Dictionary<NodeData.NodeType, List<NodeData>> nodesByType = new Dictionary<NodeData.NodeType, List<NodeData>>();

        foreach (NodeData node in allNodes)
        {
            NodeData.NodeType key = node.nodeType;

            if (!nodesByType.ContainsKey(key))
            {
                nodesByType[key] = new List<NodeData>();
            }

            nodesByType[key].Add(node);
        }

        return nodesByType;
    }

    private void PrintMap()
    {
        for (int height = 0; height <= mapHeight; height++) 
        {
            if (map.ContainsKey(height)) 
            {
                foreach (NodeData node in map[height])
                {
                    if (node.children != null && node.children.Count > 0)
                    {
                        var childNames = node.children.OrderBy(c => c.nodeHeigth).Select(c => c.name).ToList();
                        Debug.Log($"Node: {node.name}, Children: {string.Join(", ", childNames)}, Height: {node.nodeHeigth} ");
                    }
                    else
                    {
                        Debug.Log($"Node: {node.name}, Height: {node.nodeHeigth} ");
                    }
                }
            }
            else
            {
                Debug.Log($"Height {height}: No nodes");
            }
        }
    }
}
