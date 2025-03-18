using UnityEngine;
using UnityEngine.UI;

public class QuestIcon : MonoBehaviour
{
    [SerializeField]
    private Image completedImage;
    private GameObject informationCanvas;
    private QuestData quest;

    public void SetInformationCanvas(GameObject _informationCanvas)
    {
        informationCanvas = _informationCanvas;
    }

    public void ActivateInformationCanvas()
    {
        informationCanvas.SetActive(true);
        informationCanvas.GetComponent<QuestInformationCanvas>().UpdateText(quest.title, quest.description);
        QuestManager.Instance.currentQuest = quest;
    }

    public void SetQuestData(QuestData _quest)
    {
        quest = _quest;
        completedImage.gameObject.SetActive(_quest.completed);
    }
}
