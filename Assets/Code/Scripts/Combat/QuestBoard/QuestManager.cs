using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<QuestData> allQuests;
    public List<QuestData> availableQuests;
    public QuestData currentQuest;

    public StartMisionAnimation misionAnimation;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            UnLockLevels();
    }

    private void UnLockLevels()
    {
        foreach (QuestData quests in allQuests)
            quests.completed = true;
    }

    public void UpdateAvailableQuests()
    {
        availableQuests = new List<QuestData>();
        foreach (QuestData quest in allQuests)
        {
            bool addQuest = true;
            foreach (QuestData questToComplete in quest.questsToComplete)
            {
                if (!questToComplete.completed)
                {
                    addQuest = false;
                    break;
                }
            }

            if (addQuest)
                AddAvailableQuest(quest);
        }
    }

    public void AddAvailableQuest(QuestData newQuest)
    {
        availableQuests.Add(newQuest);
    }

    public void AcceptQuest()
    {
        NodeManager.instance.SetData(currentQuest);
        misionAnimation.StartAnimation();
    }
}
