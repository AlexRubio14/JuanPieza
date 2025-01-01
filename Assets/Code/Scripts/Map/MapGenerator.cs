using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Parameters")]
    private int mapHeight;

    [Header("Node Data")]
    [SerializeField] private List<NodeData> allNodes;
    [SerializeField] private NodeData startNode;
    [SerializeField] private NodeData boss;
    private Dictionary<int, List<NodeData>> nodes;
    private Dictionary<int, List<NodeData>> map;

    private void Start()
    {
        nodes = new Dictionary<int, List<NodeData>>();
        map = new Dictionary<int, List<NodeData>>();

        mapHeight = boss.difficult;

        GenerateMap();
    }

    public void GenerateMap()
    {
        nodes = OrderNodes();
        map[startNode.difficult] = new List<NodeData> { startNode };

        SetRandomChildrens(startNode);

        map[mapHeight] = new List<NodeData> { boss };

        Debug.Log($"Boss Node: {boss.name}, Children: None (Boss is the final node)");
    }

    private void SetRandomChildrens(NodeData node)
    {
        int nextDifficulty = node.difficult + 1;

        if (nextDifficulty == mapHeight)
        {
            node.children = new List<NodeData> { boss };
            Debug.Log($"Node: {node.name}, Children: {string.Join(", ", node.children.Select(c => c.name))}");
            return;
        }

        List<NodeData> availableNodes = nodes[nextDifficulty];

        List<NodeData> selectedChildren = availableNodes.OrderBy(x => Random.value).Take(2).ToList();

        node.children = selectedChildren;

        if (!map.ContainsKey(nextDifficulty))
        {
            map[nextDifficulty] = new List<NodeData>();
        }
        foreach (var child in selectedChildren)
        {
            if (!map[nextDifficulty].Contains(child))
            {
                map[nextDifficulty].Add(child);
            }
        }

        Debug.Log($"Node: {node.name}, Children: {string.Join(", ", node.children.Select(c => c.name))}");

        foreach (NodeData child in selectedChildren)
        {
            SetRandomChildrens(child);
        }
    }

    private Dictionary<int, List<NodeData>> OrderNodes()
    {
        Dictionary<int, List<NodeData>> nodesByDifficulty = new Dictionary<int, List<NodeData>>();

        foreach (NodeData node in allNodes)
        {
            int key = node.difficult;

            if (!nodesByDifficulty.ContainsKey(key))
            {
                nodesByDifficulty[key] = new List<NodeData>();
            }

            nodesByDifficulty[key].Add(node);
        }

        return nodesByDifficulty;
    }
}
