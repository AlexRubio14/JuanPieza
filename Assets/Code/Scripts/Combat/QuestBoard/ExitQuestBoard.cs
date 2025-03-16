using UnityEngine;
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
    }
}
