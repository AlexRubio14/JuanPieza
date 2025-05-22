using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
   [SerializeField] private Image progressBarImage;

    private void Start()
    {
        SetProgress(0, 1);
    }
    public void SetProgress(float progress, float maxProgress)
    {
       progressBarImage.fillAmount = progress / maxProgress;
    }
    public void EnableProgressBar(bool enable)
    {
       gameObject.SetActive(enable);
    }
}
