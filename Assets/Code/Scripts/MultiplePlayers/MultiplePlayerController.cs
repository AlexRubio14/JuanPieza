using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplePlayerController : MonoBehaviour
{
    private PlayersReadyController playerReadyController;

    private void Awake()
    {
        playerReadyController = FindFirstObjectByType<PlayersReadyController>();
    }


    public void JoinnedPlayer(PlayerInput obj)
    {
        playerReadyController.AddPlayer(obj);
    }
}
