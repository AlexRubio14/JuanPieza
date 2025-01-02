using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    private NodeData currentLevel;
    private List<NodeData> childrenLevel = new List<NodeData>();
    private Dictionary<int, List<NodeData>> map = new Dictionary<int, List<NodeData>>();
    private int mapHeight;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Update()
    {
        if(currentLevel.children.Count > 1)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                UpdateCurrentLevel(childrenLevel[0]);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                UpdateCurrentLevel(childrenLevel[1]);
            }
        }
        else if(currentLevel.children.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                UpdateCurrentLevel(childrenLevel[0]);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                UpdateCurrentLevel(childrenLevel[0]);
            }
        }
    }

    private void UpdateCurrentLevel(NodeData _currentLevel)
    {
        currentLevel = _currentLevel;
        childrenLevel = currentLevel.children;
        Debug.Log($"Node: {currentLevel.name},Height: {currentLevel.nodeHeigth}");
    }

    public void SetMap(Dictionary<int, List<NodeData>> _map)
    {
        map = _map;

        List<NodeData> levelZeroNodes = map[0];
        foreach (var node in levelZeroNodes)
        {
            UpdateCurrentLevel(node);
        }
    }

    private void PrintMap()
    {
        foreach (var level in map)
        {
            foreach (var node in level.Value)
            {
                if (node.nodeType == NodeData.NodeType.BOSS)
                {
                    mapHeight = level.Key;
                    break;
                }
            }
        }

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
