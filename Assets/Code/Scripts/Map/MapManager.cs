using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [Header("Level")]
    [SerializeField] private NodeData firtLevel;
    [SerializeField] private NodeData bossLevel;
    [SerializeField] private List<NodeData> battleLevels;
    [SerializeField] private List<NodeData> shopLevels;
    [SerializeField] private List<NodeData> eventLevels;
    private NodeData currentLevel;
    private List<NodeData> levelChilds;

    [Header("percentage")]
    [SerializeField] private float battleInitPercentage;
    [SerializeField] private float shopInitPercentage;
    [SerializeField] private float eventInitPercentage;
    private float battlePercentage;
    private float shopPercentage;
    private float eventPercentage;

    [Header("Height")]
    [SerializeField] private int mapMaxHeight;
    private int mapHeight;

    List<Votation> votations = new List<Votation>();
    private int choosenChild;

    [Header("Votation Timer")]
    [SerializeField] private float voteTime;
    private float currentTime;
    private bool startVoteTimer;

    [Header("Votation Direction")]
    [SerializeField] private float firstPointZ;
    [SerializeField] private float secondPointZ;
    [SerializeField] private float secondPointX;

    public bool isVoting;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitMap();
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void InitMap()
    {
        mapMaxHeight = 0;
        UpdateCurrentLevel(firtLevel, 0);
        levelChilds = new List<NodeData>();

        battlePercentage = battleInitPercentage;
        shopPercentage = shopInitPercentage;
        eventPercentage = eventInitPercentage;
    }

    private void Update()
    {
        Timer();
        if(Input.GetKeyUp(KeyCode.X)) 
        {
            RandomChild();
            Debug.Log(levelChilds[0].name + "    " + levelChilds[1].name);
        }
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
                StartMovingShip();
                DesactiveInformation();
            }
        }
    }

    private void DesactiveInformation()
    {
        foreach (var vot in votations)
            vot.gameObject.SetActive(false);

        VotationCanvasManager.Instance.SetVotationUIState(false);
        currentTime = 0;
        startVoteTimer = false;
    }

    private void StartMovingShip()
    {
        List<Vector3> positions = new List<Vector3>();
        ShipCurve shipCurve = FindFirstObjectByType<ShipCurve>();
        GameObject ship = shipCurve.gameObject;

        positions.Add(ship.transform.position);
        positions.Add(new Vector3(ship.transform.position.x, ship.transform.position.y, ship.transform.position.z + firstPointZ));
       
        switch(choosenChild)
        {
            case 0:
                positions.Add(new Vector3(ship.transform.position.x - secondPointX, ship.transform.position.y, ship.transform.position.z + secondPointZ));
                break;
            case 1:
                positions.Add(new Vector3(ship.transform.position.x + secondPointX, ship.transform.position.y, ship.transform.position.z + secondPointZ));
                break;
            default:
                positions.Add(new Vector3(ship.transform.position.x, ship.transform.position.y, ship.transform.position.z + secondPointZ));
                break;
        }

        shipCurve.SetStartMovement(true, positions);
    }

    private void UpdateUIPlayerText()
    {
        VotationCanvasManager.Instance.SetInformationPlayers(votations);
        VotationCanvasManager.Instance.SetTimerUiInformation(currentTime, voteTime);
    }


    private void VotationDecision()
    {
        if (votations[0].GetCurrentsPlayer().Count == votations[1].GetCurrentsPlayer().Count)
        {
            RandomDecision();
            return;
        }

        UpdateLevelCondition(votations[0].GetCurrentsPlayer().Count > votations[1].GetCurrentsPlayer().Count);
    }

    private void RandomDecision()
    {
        float randomChance = Random.value;
        UpdateLevelCondition(randomChance < 0.5f);
    }

    private void UpdateLevelCondition(bool state)
    {
        if (state)
            UpdateCurrentLevel(levelChilds[votations[0].GetDirecctionValue()], 0);
        else
            UpdateCurrentLevel(levelChilds[votations[1].GetDirecctionValue()], 1);
    }

    private void UpdateCurrentLevel(NodeData _currentLevel, int index)
    {
        currentLevel = _currentLevel;
        choosenChild = index;
        mapHeight++;
    }

    private void RandomChild()
    {
        levelChilds = new List<NodeData>();
        NodeData.NodeType firstChildType = GetRandomNodeType(null);
        NodeData.NodeType secondChildType = GetRandomNodeType(firstChildType); 

        levelChilds.Add(GetRandomNode(firstChildType));
        levelChilds.Add(GetRandomNode(secondChildType));
    }
    private NodeData.NodeType GetRandomNodeType(NodeData.NodeType? excludedType)
    {
        float battleWeight = battlePercentage;
        float shopWeight = shopPercentage;
        float eventWeight = eventPercentage;

        if (currentLevel.nodeType == NodeData.NodeType.SHOP || excludedType == NodeData.NodeType.SHOP)
            shopWeight = 0f;  

        if (currentLevel.nodeType == NodeData.NodeType.EVENT || excludedType == NodeData.NodeType.EVENT)
            eventWeight = 0f; 

        float totalWeight = battleWeight + shopWeight + eventWeight;
        float randomValue = Random.value * totalWeight;

        if (randomValue < battleWeight)
            return NodeData.NodeType.BATTLE;
        else if (randomValue < battleWeight + shopWeight)
            return NodeData.NodeType.SHOP;
        else
            return NodeData.NodeType.EVENT;
    }


    private NodeData GetRandomNode(NodeData.NodeType type)
    {
        if (type == NodeData.NodeType.BATTLE)
        {
            battlePercentage -= 0.1f;
            shopPercentage += 0.07f;
            eventPercentage += 0.03f;
            return battleLevels[Random.Range(0, battleLevels.Count)];
        }       
        if (type == NodeData.NodeType.SHOP)
        {
            ResetPercentageValues();
            return shopLevels[Random.Range(0, shopLevels.Count)];
        }
        ResetPercentageValues();
        return eventLevels[Random.Range(0, eventLevels.Count)];
    }

    private void ResetPercentageValues()
    {
        battlePercentage = battleInitPercentage;
        shopPercentage = shopInitPercentage;
        eventPercentage = eventInitPercentage;
    }

    public void SetVotations(List<Votation> _votations)
    {
        CameraManager.Instance.SetSailCamera(true);
        CameraManager.Instance.SetSimpleCamera(false);
        
        isVoting = true;
        votations = _votations;
    }

    private void ActiveUI()
    {
        VotationCanvasManager.Instance.SetVotationUIState(true);
        VotationCanvasManager.Instance.SetInformationDestination(levelChilds);
    }

    public void InitVotations()
    {
        CameraManager.Instance.SetSailCamera(false);

        RandomChild();

        if (mapHeight == mapMaxHeight)
        {
            UpdateCurrentLevel(bossLevel, 3);
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

    public NodeData GetCurrentLevel()
    {
        return currentLevel;
    }
}
