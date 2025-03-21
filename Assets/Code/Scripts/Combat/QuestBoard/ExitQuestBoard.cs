using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ExitQuestBoard : MonoBehaviour
{
    private Button exitButton;
    [SerializeField] private GameObject mapCanvas;

    private void Awake()
    {
        exitButton = GetComponent<Button>();

        exitButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        mapCanvas.SetActive(false);
        foreach ((PlayerInput, SinglePlayerController) player in PlayersManager.instance.players)
        {
            player.Item1.SwitchCurrentActionMap("Gameplay");
        }
    }
}
