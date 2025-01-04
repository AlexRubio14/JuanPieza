using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    private LevelNode currentLevel;
    private List<LevelNode> childrenLevel = new List<LevelNode>();
    private Dictionary<int, List<LevelNode>> map = new Dictionary<int, List<LevelNode>>();
    private int mapHeight;

    List<Votation> votations = new List<Votation>();

    [Header("VotationTimer")]
    [SerializeField] private float voteTime;
    private float currentTime;
    private bool startVoteTimer;

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
        CanvasManager.Instance.SetVotationUIState(false);
    }

    private void Update()
    {
        Timer();
    }

    private void Timer()
    {
        if (startVoteTimer)
        {
            currentTime += Time.deltaTime;
            UpdateUIPlayerText();
            if (currentTime >= voteTime)
            {
                VotationDecision();
                foreach (var vot in votations)
                    vot.gameObject.SetActive(false);
                currentTime = 0;
                foreach (var player in PlayersManager.instance.ingamePlayers)
                    player.votationDone = false;
                CanvasManager.Instance.SetVotationUIState(false);
                startVoteTimer = false;
            }
        }
    }

    private void UpdateUIPlayerText()
    {
        CanvasManager.Instance.SetInformationPlayers(votations);
        CanvasManager.Instance.SetTimerUiInformation(currentTime, voteTime);
    }


    private void VotationDecision()
    {
        if (votations[0].GetCurrentsPlayer().Count == votations[1].GetCurrentsPlayer().Count)
        {
            RandomDecision();
            return;
        }

        if (votations[0].GetCurrentsPlayer().Count > votations[1].GetCurrentsPlayer().Count)
            UpdateCurrentLevel(currentLevel._nodeChildren[votations[0].GetDirecctionValue()]);
        else
            UpdateCurrentLevel(currentLevel._nodeChildren[votations[1].GetDirecctionValue()]);
    }

    private void RandomDecision()
    {
        float randomChance = Random.value;
        if(randomChance < 0.5f)
            UpdateCurrentLevel(currentLevel._nodeChildren[votations[0].GetDirecctionValue()]);
        else
            UpdateCurrentLevel(currentLevel._nodeChildren[votations[1].GetDirecctionValue()]);
    }

    private void UpdateCurrentLevel(LevelNode _currentLevel)
    {
        currentLevel = _currentLevel;
        childrenLevel = currentLevel._nodeChildren;
        var childNames = currentLevel._nodeChildren.OrderBy(c => c._nodeHeigth).Select(c => c._node.name).ToList();
        Debug.Log($"Node: {currentLevel._node.name},Height: {currentLevel._nodeHeigth},Children: {string.Join(", ", childNames)}");
    }

    public void SetMap(Dictionary<int, List<LevelNode>> _map)
    {
        map = _map;

        List<LevelNode> levelZeroNodes = map[0];
        foreach (var node in levelZeroNodes)
        {
            UpdateCurrentLevel(node);
        }
    }

    public void SetVotations(List<Votation> _votations)
    {
        //Camera lerp

        votations = _votations;
        InitVotations();
    }

    private void ActiveUI()
    {
        CanvasManager.Instance.SetVotationUIState(true);
        CanvasManager.Instance.SetInformationDestination(currentLevel);
    }

    public void InitVotations()
    {
        if (currentLevel._nodeChildren.Count == 1)
        {
            UpdateCurrentLevel(currentLevel._nodeChildren[0]);
            return;
        }

        foreach (var vot in votations)
        {
            vot.CleanPlayerList();
            vot.gameObject.SetActive(true);
        }

        ActiveUI();
        startVoteTimer = true;
    }

    private void PrintMap()
    {
        foreach (var level in map)
        {
            foreach (var node in level.Value)
            {
                if (node._node.nodeType == NodeData.NodeType.BOSS)
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
                foreach (LevelNode node in map[height])
                {
                    string probabilities = $"Battle: {node._nodeBattlePercentage:P1}, Shop: {node._nodeShopPercentage:P1}, Event: {node._nodeEventPercentage:P1}";

                    if (node._nodeChildren != null && node._nodeChildren.Count > 0)
                    {
                        var childNames = node._nodeChildren.OrderBy(c => c._nodeHeigth).Select(c => c._node.name).ToList();
                        Debug.Log($"Node: {node._node.name}, Probabilities: {probabilities}, Children: {string.Join(", ", childNames)}, Height: {node._nodeHeigth}");
                    }
                    else
                    {
                        Debug.Log($"Node: {node._node.name}, Probabilities: {probabilities}, Height: {node._nodeHeigth}");
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
