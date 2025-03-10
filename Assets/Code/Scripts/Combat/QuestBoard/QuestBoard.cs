using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestBoard : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject informationCanvas;

    private void OnEnable()
    {
        QuestManager.Instance.UpdateAvailableQuests();

        foreach (QuestData quest in QuestManager.Instance.availableQuests)
        {
            GameObject questButton = Instantiate(buttonPrefab, transform);
            questButton.transform.localPosition = new Vector3(quest.positionInMap.x, quest.positionInMap.y, 0);
            questButton.GetComponent<QuestIcon>().SetInformationCanvas(informationCanvas);
            questButton.GetComponent<QuestIcon>().SetQuestData(quest);
        }   
    }

    public void AcceptQuest()
    {
        NodeManager.instance.SetData(QuestManager.Instance.currentQuest);
        SceneManager.LoadScene("Battle");
    }
}
