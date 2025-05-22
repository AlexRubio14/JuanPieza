using UnityEngine;

public class ShowMessageRepair : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject boxMessage;
    [SerializeField] private GameObject[] holeMessages;

    private void Start()
    {
        boxMessage.SetActive(false);
        foreach(GameObject hole in holeMessages)
            hole.SetActive(false);
    }

    public void ActiveBoxMessage()
    {
        boxMessage.SetActive(true);
    }

    public void TakeHarmer()
    {
        boxMessage.SetActive(false);
        foreach (GameObject hole in holeMessages)
            hole.SetActive(true);
        Destroy(this);
    }

}
