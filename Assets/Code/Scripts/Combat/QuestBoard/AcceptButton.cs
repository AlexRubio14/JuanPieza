using UnityEngine;
using UnityEngine.UI;

public class AcceptButton : MonoBehaviour
{
    private Button acceptButton;

    private void Awake()
    {
        acceptButton = GetComponent<Button>();

        acceptButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        QuestManager.Instance.AcceptQuest();
    }
}
