using System;
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

    [Space, SerializeField]
    private bool isHub;
    [SerializeField]
    private HubPlayerController hubPlayerController;

    // Start is called before the first frame update
    private string nextScene;
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
    }

    public void AddPlayer(PlayerInput _newPlayer)
    {
        if (isHub)
            AddPlayerToHub(_newPlayer);
        else
            AddPlayerToMenu(_newPlayer);
    }   

    private void AddPlayerToHub(PlayerInput _newPlayer)
    {
        (PlayerInput input, SinglePlayerController singlePlayer) newPlayer = (_newPlayer, _newPlayer.GetComponent<SinglePlayerController>());
        newPlayer.singlePlayer.currentPlayerSelectorObject.SetActive(false);
        PlayersManager.instance.players.Add(newPlayer);
        int playerIndex = PlayersManager.instance.players.IndexOf(newPlayer);

        _newPlayer.GetComponent<GameInput>().playerReference = playerIndex;
        SetPlayerInputEvents(_newPlayer);

        hubPlayerController.AddPlayerToHub(playerIndex);
    }
    private void AddPlayerToMenu(PlayerInput _newPlayer)
    {
        (PlayerInput input, SinglePlayerController singlePlayer) newPlayer = (_newPlayer, _newPlayer.GetComponent<SinglePlayerController>());
        PlayersManager.instance.players.Add(newPlayer);
        int playerIndex = PlayersManager.instance.players.IndexOf(newPlayer);
        PlacePlayerOnMenu(playerIndex);
        SetPlayerInputEvents(_newPlayer);
        DisplayStartGameButton();

        _newPlayer.GetComponent<GameInput>().playerReference = playerIndex;

        SkinnedMeshRenderer[] renderers = _newPlayer.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            renderer.material = PlayersManager.instance.characterMat[_newPlayer.playerIndex];
        }

        MeshRenderer hatRenderer = _newPlayer.gameObject.GetComponentInChildren<MeshRenderer>();
        hatRenderer.material = PlayersManager.instance.characterMat[_newPlayer.playerIndex];
    }
    private void PlacePlayerOnMenu(int _playerIndex)
    {
        //Ocultar los botones de unirse en el lado que se 
        joinGameButtonsUI[_playerIndex].SetActive(false);
        //mover el player al punto exacto del menu
        PlayersManager.instance.players[_playerIndex].Item1.transform.position = playerUIPos[_playerIndex].transform.position;
        PlayersManager.instance.players[_playerIndex].Item1.transform.forward = -playerUIPos[_playerIndex].transform.forward;

    }
    private void SetPlayerInputEvents(PlayerInput _playerInput)
    {
        _playerInput.currentActionMap.FindAction("StartGame").performed += StartGameEvent;
        _playerInput.currentActionMap.FindAction("Exit").performed += RemovePlayerEvent;
    }

    public void RemovePlayerEvent(InputAction.CallbackContext obj)
    {
        //Destruimos el ultimo player
        int playerToDestroyID = PlayersManager.instance.players.Count - 1;

        PlayersManager.instance.players[playerToDestroyID].Item1.currentActionMap.FindAction("StartGame").performed -= StartGameEvent;
        PlayersManager.instance.players[playerToDestroyID].Item1.currentActionMap.FindAction("Exit").performed -= RemovePlayerEvent;

        Destroy(PlayersManager.instance.players[playerToDestroyID].Item1.gameObject);
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
            PlayersManager.instance.players[i].Item1.currentActionMap.FindAction("StartGame").performed -= StartGameEvent;
            PlayersManager.instance.players[i].Item1.currentActionMap.FindAction("Exit").performed -= RemovePlayerEvent;
            PlayersManager.instance.players[i].Item2.currentPlayerSelectorObject.SetActive(false);
        }

        SceneManager.LoadScene(nextScene);

    }
}