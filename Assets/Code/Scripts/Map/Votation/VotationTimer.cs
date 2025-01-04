using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class VotationTimer : MonoBehaviour
{
    [Header("Information")]
    [SerializeField] private TextMeshProUGUI timerInformation;
    [SerializeField] private Image image;

    public void SetTimerUi(float time, float maxTime)
    {
        timerInformation.text = ((int)(maxTime - time)).ToString();
        image.fillAmount = 1 - (time / maxTime);
    }
}
