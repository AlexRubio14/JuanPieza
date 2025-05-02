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

    public void AddPlayerToHub(int _playerId)
    {
        PlayerController controller = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
        controller.gameObject.transform.position = playersSpawnPos[_playerId].transform.position;
        controller.gameObject.name = "Player" + _playerId;
        controller.playerInput = PlayersManager.instance.players[_playerId].playerInput.GetComponent<GameInput>();

        PlayersManager.instance.players[_playerId].playerInput.SwitchCurrentActionMap("Gameplay");

        if (PlayerPrefs.HasKey(HUB_TUTORIAL) && PlayerPrefs.GetInt(HUB_TUTORIAL) == 1)
            return;


        FindAnyObjectByType<HubTutorialNPC>().PlayStarterDialogue();
        PlayerPrefs.SetInt(HUB_TUTORIAL, 1);
        PlayerPrefs.Save();
    }
}
