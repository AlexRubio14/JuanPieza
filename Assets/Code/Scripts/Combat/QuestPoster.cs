using UnityEngine;

public class QuestPoster : MonoBehaviour
{
    [SerializeField] private QuestData quest;
    [SerializeField] private QuestInfoPoster questInfoCanvas;

    private void Awake()
    {
        DeactivateCanvas();
    }

    public void ActivateCanvas()
    {
        questInfoCanvas.gameObject.SetActive(true);
        questInfoCanvas.UpdateCanvas(quest);
    }

    public void DeactivateCanvas()
    {
        questInfoCanvas.gameObject.SetActive(false);
    }

    private void UpdateCanvasInfo()
    {
        //cambiar titulo
        //cambiar foto
        //cambiar descripcion
        //cambiar cañones disponibles
    }


}
