using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayersReadyController : MonoBehaviour
{
    const int MIN_PLAYERS = 1;

    [SerializeField]
    private string sceneToLoad;

    [SerializeField]
    private MultiplePlayerController multiplayerController;

    [Space,SerializeField]
    private GameObject[] joinGameButtonsUI;
    [SerializeField]
    private GameObject startGameButton;

    [Space, SerializeField]
    private GameObject[] playerUIPos;
    private string nextScene;

    [Space, Header("Hub"), SerializeField]
    private bool isHub;
    [SerializeField]
    private HubPlayerController hubPlayerController;
    [SerializeField]
    private string HUB_TUTORIAL;
    [SerializeField]
    private GameObject hubTutorialPrefab;

    // Start is called before the first frame update
    private void Awake()
    {
        if (TestPlayerController.Instance)
        {
            nextScene = TestPlayerController.Instance.lastSceneActive;
            //Debug.Log("Load last Scene");
        }
        else
        {
            nextScene = sceneToLoad;
            //Debug.Log("Load default Scene");
        }
    }

    void Start()
    {
        if(!isHub)
            DisplayStartGameButton();
        else
            CheckHubTutorial();
    }

    public void AddPlayer(PlayerInput _newPlayer)
    {
        if (isHub)
            AddPlayerToHub(_newPlayer);
        else
            AddPlayerToMenu(_newPlayer);
    }
    private void SetPlayerInputEvents(PlayerInput _playerInput)
    {
        _playerInput.currentActionMap.FindAction("StartGame").performed += StartGameEvent;
        _playerInput.currentActionMap.FindAction("Exit").performed += RemovePlayerEvent;
    }

    #region Test Player Selector
    private void AddPlayerToMenu(PlayerInput _newPlayer)
    {
        PlayersManager.PlayerData newPlayer = new PlayersManager.PlayerData();
        newPlayer.playerInput = _newPlayer;
        newPlayer.singlePlayer = _newPlayer.GetComponent<SinglePlayerController>();
        newPlayer.gameInput = _newPlayer.GetComponent<GameInput>();
        newPlayer.rumbleController = _newPlayer.GetComponent<RumbleController>();
        PlayersManager.instance.players.Add(newPlayer);
        int playerIndex = PlayersManager.instance.players.IndexOf(newPlayer);
        PlacePlayerOnMenu(playerIndex);
        SetPlayerInputEvents(_newPlayer);
        DisplayStartGameButton();

        _newPlayer.GetComponent<GameInput>().playerReference = playerIndex;

        newPlayer.singlePlayer.currentColor = -1;
        int colorIndex = PlayersManager.instance.GetNextMaterial(-1);
        newPlayer.singlePlayer.ChangePlayerColor(colorIndex);
    }
    private void PlacePlayerOnMenu(int _playerIndex)
    {
        //Ocultar los botones de unirse en el lado que se 
        joinGameButtonsUI[_playerIndex].SetActive(false);
        //mover el player al punto exacto del menu
        PlayersManager.instance.players[_playerIndex].playerInput.transform.position = playerUIPos[_playerIndex].transform.position;
        PlayersManager.instance.players[_playerIndex].playerInput.transform.forward = -playerUIPos[_playerIndex].transform.forward;

    }
   

    public void RemovePlayerEvent(InputAction.CallbackContext obj)
    {
        //Destruimos el ultimo player
        int playerToDestroyID = PlayersManager.instance.players.Count - 1;

        PlayersManager.instance.players[playerToDestroyID].playerInput.currentActionMap.FindAction("StartGame").performed -= StartGameEvent;
        PlayersManager.instance.players[playerToDestroyID].playerInput.currentActionMap.FindAction("Exit").performed -= RemovePlayerEvent;

        Destroy(PlayersManager.instance.players[playerToDestroyID].playerInput.gameObject);
        //Lo quitamos de las listas
        PlayersManager.instance.players.RemoveAt(playerToDestroyID);
        //Hacemos aparecer de nuevo la UI
        joinGameButtonsUI[playerToDestroyID].SetActive(true);
    }
    public void StartGameEvent(InputAction.CallbackContext obj)
    {
        if (PlayersManager.instance.players.Count >= MIN_PLAYERS)
            StartGame();
    }
    private void DisplayStartGameButton()
    {
        startGameButton.SetActive(PlayersManager.instance.players.Count >= MIN_PLAYERS);
    }

    private void StartGame()
    {
        for (int i = 0; i < PlayersManager.instance.players.Count; i++)
        {
            PlayersManager.instance.players[i].playerInput.currentActionMap.FindAction("StartGame").performed -= StartGameEvent;
            PlayersManager.instance.players[i].playerInput.currentActionMap.FindAction("Exit").performed -= RemovePlayerEvent;
            PlayersManager.instance.players[i].singlePlayer.currentPlayerSelectorObject.SetActive(false);
        }

        SceneManager.LoadScene(nextScene);

    }
    #endregion


    #region Hub
    private void AddPlayerToHub(PlayerInput _newPlayer)
    {
        PlayersManager.PlayerData newPlayer = new PlayersManager.PlayerData();
        bool playerExists = false;
        foreach (PlayersManager.PlayerData item in PlayersManager.instance.players)
        {
            if (_newPlayer == item.playerInput)
            {
                newPlayer = item;
                playerExists = true;
                break;
            }
        }


        if (!playerExists)
        {
            newPlayer.playerInput = _newPlayer;
            newPlayer.singlePlayer = _newPlayer.GetComponent<SinglePlayerController>();
            newPlayer.gameInput = _newPlayer.GetComponent<GameInput>();
            newPlayer.rumbleController = _newPlayer.GetComponent<RumbleController>();
            PlayersManager.instance.players.Add(newPlayer);
            newPlayer.singlePlayer.currentColor = -1;
            int colorIndex = PlayersManager.instance.GetNextMaterial(-1);
            newPlayer.singlePlayer.ChangePlayerColor(colorIndex);
        }

        int playerIndex = PlayersManager.instance.players.IndexOf(newPlayer);
        _newPlayer.GetComponent<GameInput>().playerReference = playerIndex;

        newPlayer.singlePlayer.currentPlayerSelectorObject.SetActive(false);

        hubPlayerController.AddPlayerToHub(playerIndex);

        //Ocultar los botones de unirse en el lado que se 
        joinGameButtonsUI[playerIndex].SetActive(false);
    }
    private void CheckHubTutorial()
    {
        if (PlayerPrefs.HasKey(HUB_TUTORIAL) && PlayerPrefs.GetInt(HUB_TUTORIAL) == 1)
            return;

        //Instanciar Tutorial
        Instantiate(hubTutorialPrefab, Vector3.zero, Quaternion.identity);
    }
    #endregion
}