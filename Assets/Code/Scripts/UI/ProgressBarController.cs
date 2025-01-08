using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
   [SerializeField] private Image progressBarImage;

   public void SetProgress(float progress, float maxProgress)
   {
      progressBarImage.fillAmount = progress / maxProgress;
   }

   public void EnableProgressBar(bool enable)
   {
      gameObject.SetActive(enable);
   }
}
