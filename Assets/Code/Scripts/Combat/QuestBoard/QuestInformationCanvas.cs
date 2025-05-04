using TMPro;
using UnityEngine;

public class QuestInformationCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    //faltan los componentes

    public void UpdateText(string _title)
    {
        title.text = _title;
    }
}
