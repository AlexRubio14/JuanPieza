using UnityEngine;

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
            PlayersManager.instance.players[i].Item1.SwitchCurrentActionMap("Gameplay");

            SkinnedMeshRenderer[] renderers = controller.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (SkinnedMeshRenderer renderer in renderers)
            {
                renderer.material = PlayersManager.instance.characterMat[i];
            }

            MeshRenderer hatRenderer = controller.gameObject.GetComponentInChildren<MeshRenderer>();
            hatRenderer.material = PlayersManager.instance.characterMat[i];

            if (cameraCont)
                cameraCont.AddPlayer(controller.gameObject);
        }

        


    }
}
