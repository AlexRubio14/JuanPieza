using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager instance;

    [field: SerializeField]
    public BattleQuestNodeData battleInformation { get; private set; }
    [field: SerializeField]
    public QuestData questShip { get; private set; }

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


}
