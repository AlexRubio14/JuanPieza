using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplePlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayersReadyController playerReadyController;

    public void JoinnedPlayer(PlayerInput obj)
    {
        playerReadyController.AddPlayer(obj);
    }
}
