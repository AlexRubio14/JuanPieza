using System.Collections.Generic;
using UnityEngine;

public class HubPlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private List<Transform> playersSpawnPos;

    [Space, SerializeField]
    private string HUB_TUTORIAL;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < PlayersManager.instance.players.Count; i++)
            AddPlayerToHub(i);

    }

    public void AddPlayerToHub(int _playerId)
    {
        PlayerController controller = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
        controller.gameObject.transform.position = playersSpawnPos[_playerId].transform.position;
        controller.gameObject.name = "Player" + _playerId;
        controller.playerInput = PlayersManager.instance.players[_playerId].Item1.GetComponent<GameInput>();

        PlayersManager.instance.players[_playerId].Item1.SwitchCurrentActionMap("Gameplay");

        SkinnedMeshRenderer[] renderers = controller.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            renderer.material = PlayersManager.instance.characterMat[_playerId];
        }

        MeshRenderer hatRenderer = controller.gameObject.GetComponentInChildren<MeshRenderer>();
        hatRenderer.material = PlayersManager.instance.characterMat[_playerId];

        if (PlayerPrefs.HasKey(HUB_TUTORIAL) && PlayerPrefs.GetInt(HUB_TUTORIAL) == 1)
            return;


        FindAnyObjectByType<HubTutorialNPC>().PlayStarterDialogue();
        PlayerPrefs.SetInt(HUB_TUTORIAL, 1);
        PlayerPrefs.Save();
    }
}
