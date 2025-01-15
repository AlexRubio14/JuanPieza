using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelNode
{
    public NodeData _node;
    public List<LevelNode> _nodeChildren;
    public int _nodeHeigth;
    public float _nodeBattlePercentage;
    public float _nodeShopPercentage;
    public float _nodeEventPercentage;

    public static LevelNode CreateLevelNode(NodeData node, int height, List<float> probabilities)
    {
        LevelNode newLevelNode = new LevelNode();
        newLevelNode._node = node?.CretaeNode();
        newLevelNode._nodeChildren = new List<LevelNode>();
        newLevelNode._nodeHeigth = height;
        newLevelNode._nodeBattlePercentage = probabilities[0];
        newLevelNode._nodeShopPercentage = probabilities[1];
        newLevelNode._nodeEventPercentage = probabilities[2];

        return newLevelNode;
    }
}

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
    private Dictionary<int, List<LevelNode>> map = new Dictionary<int, List<LevelNode>>(); 

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

        LevelNode bossNode = CreateNodeMap(boss, mapHeight, probabilites);
        LevelNode _startNode = CreateNodeMap(startNode, currentHeight, probabilites);

        SetRandomChildrens(_startNode, currentHeight, bossNode);
        manager.SetMap(map);
    }

    private void SetRandomChildrens(LevelNode node, int nodeHeight, LevelNode boss)
    {
        nodeHeight++;

        CreateMapHeight(nodeHeight);

        if (nodeHeight == mapHeight)
        {
            node._nodeChildren = new List<LevelNode> { boss };
            return;
        }

        node._nodeChildren = CreateNodeChildren(nodeHeight, node);

        foreach (LevelNode child in node._nodeChildren)
        {
            SetRandomChildrens(child, child._nodeHeigth, boss);
        }
    }

    private (NodeData.NodeType, List<float>) GetNodeTypeBasedOnProbability(LevelNode node)
    {
        float randomChance = Random.value;
        if (randomChance <= node._nodeBattlePercentage)
        {
            List<float> values = GetNodesProbability(new List<float>() { node._nodeBattlePercentage, node._nodeShopPercentage, node._nodeEventPercentage });
            return (NodeData.NodeType.BATTLE, new List<float>() { values[0], values[1], values[2] });
        }
        else if (randomChance <= node._nodeBattlePercentage + node._nodeShopPercentage)
        {
            List<float> values = GetNodesProbability(new List<float>() { node._nodeShopPercentage, node._nodeBattlePercentage, node._nodeEventPercentage });
            return (NodeData.NodeType.SHOP, new List<float>() { values[1], values[0], values[2] });
        }
        else
        {
            List<float> values = GetNodesProbability(new List<float>() { node._nodeEventPercentage, node._nodeBattlePercentage, node._nodeShopPercentage });
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
            map[height] = new List<LevelNode>();
        }
    }

    private LevelNode CreateNodeMap(NodeData node, int height, List<float> probabilities) 
    {
        LevelNode newNode = LevelNode.CreateLevelNode(node, height, probabilities);
        map[height].Add(newNode);

        return newNode;
    }

    private List<LevelNode> CreateNodeChildren(int nodeHeight, LevelNode node)
    {
        List<LevelNode> children = new List<LevelNode>();
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
