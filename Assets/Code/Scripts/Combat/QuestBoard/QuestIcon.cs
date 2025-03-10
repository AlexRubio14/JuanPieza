using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;
    [SerializeField] private float speed;

    private GameObject informationCanvas;
    private QuestData quest;

    void Update()
    {
        float scale = Mathf.PingPong(Time.time * speed, maxScale - minScale) + minScale;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void SetInformationCanvas(GameObject _informationCanvas)
    {
        informationCanvas = _informationCanvas;
    }

    public void ActivateInformationCanvas()
    {
        informationCanvas.SetActive(true);
    }

    public void SetQuestData(QuestData _quest)
    {
        quest = _quest;
    }
}
