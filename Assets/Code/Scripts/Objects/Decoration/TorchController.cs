using UnityEngine;

public class TorchController : MonoBehaviour
{
    void Start()
    {
        if(NodeManager.instance.questData.questClimete != QuestData.QuestClimete.STORM)
            gameObject.SetActive(false);
    }
}
