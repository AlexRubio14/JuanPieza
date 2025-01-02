using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class MapGenerator : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private MapManager manager;

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
        manager.SetMap(map);
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
        float randomChance = Random.value;
        if (randomChance <= node.battlePercentage)
        {
            List<float> values = GetNodesProbability(new List<float>() { node.battlePercentage, node.shopPercentage, node.eventPercentage });
            return (NodeData.NodeType.BATTLE, new List<float>() { values[0], values[1], values[2] });
        }
        else if (randomChance <= node.battlePercentage + node.shopPercentage)
        {
            List<float> values = GetNodesProbability(new List<float>() { node.shopPercentage, node.battlePercentage, node.eventPercentage });
            return (NodeData.NodeType.SHOP, new List<float>() { values[1], values[0], values[2] });
        }
        else
        {
            List<float> values = GetNodesProbability(new List<float>() { node.eventPercentage, node.battlePercentage, node.shopPercentage });
            return (NodeData.NodeType.EVENT, new List<float>() { values[1], values[2], values[0] });
        }
    }

    private List<float> GetNodesProbability(List<float> initPorbabilities)
    {
        float _divider = initPorbabilities[0] / divider;
        initPorbabilities[1] += (initPorbabilities[0] - _divider) / 2;
        initPorbabilities[2] += (initPorbabilities[0] - _divider) / 2;

        return new List<float>
        {
            _divider,
            initPorbabilities[1],
            initPorbabilities[2]
        };

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
}
