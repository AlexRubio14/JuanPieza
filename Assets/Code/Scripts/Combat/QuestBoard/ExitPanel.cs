using UnityEngine;
using UnityEngine.UI;

public class ExitPanel : MonoBehaviour
{
    private Button exitButton;
    [SerializeField] private GameObject informationCanvas;

    private void Awake()
    {
        exitButton = GetComponent<Button>();

        exitButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        QuestManager.Instance.DeactivateCanvas(informationCanvas);
    }
}
