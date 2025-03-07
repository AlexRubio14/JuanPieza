using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;

    private void OnEnable()
    {
        QuestManager.Instance.UpdateAvailableQuests();

        foreach (QuestData quest in QuestManager.Instance.availableQuests)
        {
            GameObject questButton = Instantiate(buttonPrefab, transform);
            questButton.transform.localPosition = new Vector3(quest.positionInMap.x, quest.positionInMap.y, 0);
        }   
    }
}
