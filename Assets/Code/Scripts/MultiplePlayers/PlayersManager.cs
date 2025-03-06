using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager instance;

    [field: SerializeField]
    public List<(PlayerInput playerInput, SinglePlayerController singlePlayer)> players {  get; private set; }
    public List<PlayerController> ingamePlayers {  get; private set; }

    [field: Space, SerializeField]
    public Material[] characterMat {  get; private set; }
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
        DontDestroyOnLoad(gameObject);

        players = new List<(PlayerInput, SinglePlayerController)>();
        ingamePlayers = new List<PlayerController>();
    } 

    public List<(PlayerInput, SinglePlayerController)> GetPlayers()
    {
        return players;
    }
}
