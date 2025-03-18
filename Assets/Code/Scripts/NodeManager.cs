using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager instance;

    [field: SerializeField]
    public QuestData questData { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetData(QuestData _questData)
    {
        questData = _questData;
    }

    public void CompleteQuest()
    {
        questData.completed = true;
    }

}
