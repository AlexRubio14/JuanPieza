using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipsManager : MonoBehaviour
{
    public static ShipsManager instance;

    [field: SerializeField]
    public AllyShip playerShip {  get; private set; }
    [field: SerializeField]
    public List<Ship> enemiesShips { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;

        enemiesShips = new List<Ship>();
    }

    private void Start()
    {
        ShipSceneManager.Instance.InstantiateShip();
        if(MapManager.Instance.GetCurrentLevel().nodeType == NodeData.NodeType.BATTLE)
            GenerateEnemyShip.instance.GenerateEnemiesShip();
    }

    public void AddEnemyShip(Ship _ship)
    {
        enemiesShips.Add(_ship);
    }

    public void SetShip(AllyShip ship)
    {
        playerShip = ship;
    }

    public void RemoveEnemyShip(Ship ship)
    {
        enemiesShips.Remove(ship);
        StartVotation();
    }

    public void RemoveAllyShip()
    {
        AudioManager.instance.StopLoopSound(AudioManager.instance.seagullAs);
        SceneManager.LoadScene("MainMenu");
    }

    private void StartVotation()
    {
        if (enemiesShips.Count == 0)
        {
            if(MapManager.Instance.GetCurrentLevel().nodeType == NodeData.NodeType.BATTLE)
            {
                MoneyManager.Instance.AddMoney(((BattleNodeData)MapManager.Instance.GetCurrentLevel()).levelMoney);
                VotationCanvasManager.Instance.SetMoneyText(true);
            }

            playerShip.StartVotation();
        }

    }
}
