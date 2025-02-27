using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private List<QuestData> quests;
    private List<QuestData> availableQuests;

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

    public void AddAvailableQuest(QuestData newQuest)
    {
        availableQuests.Add(newQuest);
    }
}
