using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayerController : MonoBehaviour
{
    private CameraController cameraCont;
    [SerializeField]
    private GameObject playerPrefab;

    private List<Transform> playersSpawnPos;

    private void Awake()
    {
        cameraCont = FindAnyObjectByType<CameraController>();
    }

    void Start()
    {
        StartCoroutine(WaitEndOfFrame());
        IEnumerator WaitEndOfFrame()
        {
            yield return new WaitForEndOfFrame();

            playersSpawnPos = ShipsManager.instance.playerShip.GetSpawnPoints();

            for (int i = 0; i < PlayersManager.instance.players.Count; i++)
            {
                PlayerController controller = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
                controller.gameObject.transform.position = playersSpawnPos[i].transform.position;
                controller.gameObject.name = "Player" + i;
                controller.playerInput = PlayersManager.instance.players[i].Item1.GetComponent<GameInput>();
                controller.transform.SetParent(ShipsManager.instance.playerShip.gameObject.transform, true);

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
}
