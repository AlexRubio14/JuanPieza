using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBoard : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject informationCanvas;

    private List<QuestIcon> questIcons;

    private void Start()
    {
        questIcons = new List<QuestIcon>();
        
        QuestManager.Instance.UpdateAvailableQuests();

        foreach (QuestData quest in QuestManager.Instance.availableQuests)
        {
            QuestIcon questButton = Instantiate(buttonPrefab, transform).GetComponent<QuestIcon>();
            questButton.transform.localPosition = new Vector3(quest.positionInMap.x, quest.positionInMap.y, 0);
            questButton.SetInformationCanvas(informationCanvas);
            questButton.SetQuestData(quest);

            if (questIcons.Count == 0)
                questButton.GetComponent<Button>().Select();

            questIcons.Add(questButton);
        }
    }
    public List<QuestIcon> GetQuestIcons() { return questIcons; }

    public GameObject GetInformationCanvas() { return informationCanvas; }
}
