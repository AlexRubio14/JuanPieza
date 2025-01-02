using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Parameters")]
    [SerializeField] private int mapHeight;
    [SerializeField] private List<float> percentage; 
    [SerializeField] private float divider; 
    private int currentHeight;

    [Header("Node Data")]
    [SerializeField] private List<NodeData> allNodes;
    [SerializeField] private NodeData startNode;
    [SerializeField] private NodeData boss;
    private Dictionary<NodeData.NodeType, List<NodeData>> nodes = new Dictionary<NodeData.NodeType, List<NodeData>>();
    private Dictionary<int, List<NodeData>> map = new Dictionary<int, List<NodeData>>(); 

    private void Start()
    {
        GenerateMap(); 
    }

    private void GenerateMap()
    {
        nodes = OrderNodes();

        CreateMapHeight(currentHeight);
        CreateMapHeight(mapHeight);

        List<float> probabilites = new List<float>() { percentage[0], percentage[1], percentage[2] };

        NodeData bossNode = CreateNodeMap(boss, mapHeight, probabilites);
        NodeData _startNode = CreateNodeMap(startNode, currentHeight, probabilites);

        SetRandomChildrens(_startNode, currentHeight, bossNode);
        PrintMap();
    }

    private void SetRandomChildrens(NodeData node, int nodeHeight, NodeData boss)
    {
        nodeHeight++;

        CreateMapHeight(nodeHeight);

        if (nodeHeight == mapHeight)
        {
            node.children = new List<NodeData> { boss };
            return;
        }

        node.children = CreateNodeChildren(nodeHeight, node);

        foreach (NodeData child in node.children)
        {
            SetRandomChildrens(child, child.nodeHeigth, boss);
        }
    }

    private (NodeData.NodeType, List<float>) GetNodeTypeBasedOnProbability(NodeData node)
    {
        float battlePercentage = node.battlePercentage;
        float shopPercentage = node.shopPercentage;
        float eventPercentage = node.eventPercentage;

        if (node.nodeType == NodeData.NodeType.BATTLE)
        {
            battlePercentage /= divider;
            shopPercentage += (node.battlePercentage - battlePercentage) / 2;
            eventPercentage += (node.battlePercentage - battlePercentage) / 2;
        }
        else if (node.nodeType == NodeData.NodeType.SHOP)
        {
            shopPercentage /= divider;
            battlePercentage += (node.shopPercentage - shopPercentage) / 2;
            eventPercentage += (node.shopPercentage - shopPercentage) / 2;
        }
        else if (node.nodeType == NodeData.NodeType.EVENT)
        {
            eventPercentage /= divider;
            battlePercentage += (node.eventPercentage - eventPercentage) / 2;
            shopPercentage += (node.eventPercentage - eventPercentage) / 2;
        }

        List<float> probabilities = new List<float> { battlePercentage, shopPercentage, eventPercentage };
        float randomChance = Random.value;
        if (randomChance <= battlePercentage)
        {
            return (NodeData.NodeType.BATTLE, probabilities);
        }
        else if (randomChance <= battlePercentage + shopPercentage)
        {
            return (NodeData.NodeType.SHOP, probabilities);
        }
        else
        {
            return (NodeData.NodeType.EVENT, probabilities);
        }
    } 

    private void CreateMapHeight(int height)
    {
        if (!map.ContainsKey(height))
        {
            map[height] = new List<NodeData>();
        }
    }

    private NodeData CreateNodeMap(NodeData node, int height, List<float> probabilities) 
    {
        NodeData selectedNode = node?.CretaeNode();
        selectedNode.nodeHeigth = height;
        selectedNode.battlePercentage = probabilities[0];
        selectedNode.shopPercentage = probabilities[1];
        selectedNode.eventPercentage = probabilities[2];
        map[height].Add(selectedNode);

        return selectedNode;
    }

    private List<NodeData> CreateNodeChildren(int nodeHeight, NodeData node)
    {
        List<NodeData> children = new List<NodeData>();
        HashSet<NodeData.NodeType> assignedTypes = new HashSet<NodeData.NodeType>(); 

        for (int i = 0; i < 2; i++)
        {
            (NodeData.NodeType childType, List<float> childProbabilities) = GetNodeTypeBasedOnProbability(node);

            while (assignedTypes.Contains(childType) && (childType == NodeData.NodeType.SHOP || childType == NodeData.NodeType.EVENT))
            {
                (childType, childProbabilities) = GetNodeTypeBasedOnProbability(node);
            }

            if (childType == NodeData.NodeType.SHOP || childType == NodeData.NodeType.EVENT)
            {
                assignedTypes.Add(childType);
            }

            NodeData originalNode = nodes[childType].OrderBy(x => Random.value).FirstOrDefault();
            children.Add(CreateNodeMap(originalNode, nodeHeight, childProbabilities));
        }
        return children;
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
                    string probabilities = $"Battle: {node.battlePercentage:P1}, Shop: {node.shopPercentage:P1}, Event: {node.eventPercentage:P1}";

                    if (node.children != null && node.children.Count > 0)
                    {
                        var childNames = node.children.OrderBy(c => c.nodeHeigth).Select(c => c.name).ToList();
                        Debug.Log($"Node: {node.name}, Probabilities: {probabilities}, Children: {string.Join(", ", childNames)}, Height: {node.nodeHeigth}");
                    }
                    else
                    {
                        Debug.Log($"Node: {node.name}, Probabilities: {probabilities}, Height: {node.nodeHeigth}");
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
