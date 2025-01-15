using System.Collections.Generic;
using UnityEngine;

public class LevelPlayerController : MonoBehaviour
{
    private CameraController cameraCont;
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private List<Transform> playersSpawnPos;

    private List<Vector3> safeSpawnPos;

    private void Awake()
    {
        cameraCont = FindAnyObjectByType<CameraController>();
    }

    void Start()
    {
        playersSpawnPos = ShipsManager.instance.playerShip.GetSpawnPoints();

        GetPlayerSpawnPos();

        for (int i = 0; i < PlayersManager.instance.players.Count; i++)
        {
            PlayerController controller = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();

            if (safeSpawnPos.Count > 0 && safeSpawnPos[i] != Vector3.zero)
            {
                safeSpawnPos[i] = new Vector3(safeSpawnPos[i].x, safeSpawnPos[i].y + 5, safeSpawnPos[i].z);
                controller.gameObject.transform.position = safeSpawnPos[i];
            }
            else
                controller.gameObject.transform.position = playersSpawnPos[i].transform.position;

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

    public void GetPlayerSpawnPos()
    {
        safeSpawnPos = ShipSceneManager.Instance.GetPlayersPositions();
    }
}
