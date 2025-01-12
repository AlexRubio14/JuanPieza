using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [SerializeField] private NodeData currentLevel;
    private List<NodeData> childrenLevel = new List<NodeData>();
    private Dictionary<int, List<LevelNode>> map = new Dictionary<int, List<LevelNode>>();
    private int mapHeight;

    List<Votation> votations = new List<Votation>();

    [Header("Votation Timer")]
    [SerializeField] private float voteTime;
    private float currentTime;
    private bool startVoteTimer;
    private int choosenChild;

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
        }
        else
        {
            Destroy(gameObject); 
        }
        UpdateCurrentLevel(currentLevel,0);
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
            UpdateCurrentLevel(currentLevel.children[votations[0].GetDirecctionValue()], 0);
        else
            UpdateCurrentLevel(currentLevel.children[votations[1].GetDirecctionValue()], 1);
    }

    private void UpdateCurrentLevel(NodeData _currentLevel, int index)
    {
        currentLevel = _currentLevel;
        childrenLevel = currentLevel.children;
        choosenChild = index;
    }

    public void SetMap(Dictionary<int, List<LevelNode>> _map)
    {
        //map = _map;

        //List<LevelNode> levelZeroNodes = map[0];
        //foreach (var node in levelZeroNodes)
        //{
        //    UpdateCurrentLevel(node, 0);
        //}
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
        VotationCanvasManager.Instance.SetInformationDestination(currentLevel);
    }

    public void InitVotations()
    {
        CameraManager.Instance.SetSailCamera(false);
        if (currentLevel.children.Count == 1)
        {
            UpdateCurrentLevel(currentLevel.children[0], 2);
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
