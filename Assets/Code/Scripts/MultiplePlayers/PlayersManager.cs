using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager instance;

    [field: SerializeField]
    public List<(PlayerInput, SinglePlayerController)> players {  get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
        DontDestroyOnLoad(gameObject);

        players = new List<(PlayerInput, SinglePlayerController)>();
    }
}
