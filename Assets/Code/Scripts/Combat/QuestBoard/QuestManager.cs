using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<QuestData> allQuests;
    public List<QuestData> availableQuests;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public void UpdateAvailableQuests()
    {
        availableQuests = new List<QuestData>();
        foreach (QuestData quest in allQuests)
        {
            if (quest.completed == true)
                continue;

            bool addQuest = true;
            foreach(QuestData questToComplete in quest.questsToComplete)
            {
                if (!questToComplete.completed)
                {
                    addQuest = false;
                    break;
                }
            }

            if(addQuest)
                AddAvailableQuest(quest);
        }
    }

    public void AddAvailableQuest(QuestData newQuest)
    {
        availableQuests.Add(newQuest);
    }

    public void StartQuest(BattleQuestNodeData _battleInformation, QuestData _questShip)
    {
        NodeManager.instance.SetData(_battleInformation, _questShip);
        SceneManager.LoadScene("Battle");
    }

}
