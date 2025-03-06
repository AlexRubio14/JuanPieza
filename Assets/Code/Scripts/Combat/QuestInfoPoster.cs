using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class QuestInfoPoster : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objective;
    [SerializeField] private TextMeshProUGUI description;

    public void UpdateCanvas(QuestData questData)
    {
        objective.text = questData.questObjective.ToString();

        description.text = questData.description;
    }
    
}
