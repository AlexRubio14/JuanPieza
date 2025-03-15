using UnityEngine;
using UnityEngine.UI;

public class ExitPanel : MonoBehaviour
{
    private Button exitButton;
    [SerializeField] private GameObject informationCanvas;
    [SerializeField] private QuestBoard questBoard;

    private void Awake()
    {
        exitButton = GetComponent<Button>();

        exitButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        informationCanvas.SetActive(false);
        questBoard.GetQuestIcons()[0].GetComponent<Button>().Select();
    }
}
