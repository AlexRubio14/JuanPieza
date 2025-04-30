using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestBoard : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject informationCanvas;

    private List<QuestIcon> questIcons;

    private void OnEnable()
    {
        questIcons = new List<QuestIcon>();
        
        QuestManager.Instance.UpdateAvailableQuests();

        bool selectedQuest = false;

        foreach (QuestData quest in QuestManager.Instance.availableQuests)
        {
            QuestIcon questButton = Instantiate(buttonPrefab, transform).GetComponent<QuestIcon>();
            questButton.transform.localPosition = new Vector3(quest.positionInMap.x, quest.positionInMap.y, 0);
            questButton.SetInformationCanvas(informationCanvas);
            questButton.SetQuestData(quest);

            if (quest.completed)
            {
                questButton.gameObject.transform.localScale = new Vector3(
                    questButton.gameObject.transform.localScale.x * 0.8f,
                    questButton.gameObject.transform.localScale.y * 0.8f,
                    questButton.gameObject.transform.localScale.z * 0.8f);

                Image iconImage = questButton.gameObject.GetComponent<Image>();
            }
            else
            {
                questButton.GetComponent<Button>().Select();
                selectedQuest = true;
            }

            questIcons.Add(questButton);
        }

        if(!selectedQuest)
            QuestManager.Instance.availableQuests[0].GetComponent<Button>().Select();
    }

    private void OnDisable()
    {
        foreach (QuestIcon item in questIcons)
        {
            item.gameObject.SetActive(false);
            Destroy(item.gameObject);
        }

        questIcons = null;
    }
    public List<QuestIcon> GetQuestIcons() { return questIcons; }

    public GameObject GetInformationCanvas() { return informationCanvas; }
}
