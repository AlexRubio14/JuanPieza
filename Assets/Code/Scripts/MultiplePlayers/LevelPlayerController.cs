using UnityEngine;
using UnityEngine.InputSystem;

public class LevelPlayerController : MonoBehaviour
{
    [SerializeField]
    private CameraController cameraCont;
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private Transform[] playersSpawnPos;
    void Start()
    {

        for (int i = 0; i < PlayersManager.instance.players.Count; i++)
        {
            PlayerController controller = Instantiate(playerPrefab, playersSpawnPos[i].position, Quaternion.identity).GetComponent<PlayerController>();
            controller.gameObject.name = "Player" + i;
            controller.playerInput = PlayersManager.instance.players[i].Item1.GetComponent<GameInput>();
            
            PlayersManager.instance.players[i].Item1.actions.FindActionMap("PlayerSelectMenu").Disable();
            PlayersManager.instance.players[i].Item1.actions.FindActionMap("Gameplay").Enable();

            cameraCont.AddPlayer(controller.gameObject);
        }

        


    }
}
