using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class QuestInfoPoster : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objective;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image shipImage;

    public void UpdateCanvas(QuestData questData)
    {
        objective.text = questData.questObjective.ToString();

        description.text = questData.description + "\n" + "\n";
        foreach (ResourceQuantity resource in questData.ship.resourceCuantity)
        {
            description.text += resource.quantity.ToString() + " " + resource.resource.objectName.ToString() + "\n";
        }

        shipImage.sprite = questData.ship.shipImage;
    }
    
}
