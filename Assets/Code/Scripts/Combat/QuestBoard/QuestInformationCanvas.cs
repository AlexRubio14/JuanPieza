using TMPro;
using UnityEngine;

public class QuestInformationCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    //faltan los componentes

    public void UpdateText(string _title, string _description)
    {
        title.text = _title;
        description.text = _description;
    }


}
