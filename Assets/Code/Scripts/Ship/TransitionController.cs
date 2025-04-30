using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private Image transitionImage;
    [SerializeField] private float transitionDuration = 1f;

    private Vector3 initialScale = Vector3.one;
    private Vector3 targetScale = Vector3.zero;
    private bool isStarting = false;
    private bool isEnding = false;
    private float timer = 0f;

    private void Start()
    {
        transitionImage.rectTransform.localScale = initialScale;
    }

    private void Update()
    {
        if(isStarting)
        {
            timer += Time.deltaTime;
            float progresStart = Mathf.Clamp01(timer / transitionDuration);

            transitionImage.rectTransform.localScale = Vector3.Lerp(initialScale, targetScale, progresStart);
            if (timer >= 1)
            {
                isStarting = false;
            }
            else if(timer >= 0.5)
            {
                Camera.main.GetComponent<ArriveIslandCamera>()?.SetIsArriving();
                ShipsManager.instance?.playerShip.SetArriving(true);
            }

        }
        else if(isEnding)
        {
            timer += Time.deltaTime;
            float progresStart = Mathf.Clamp01(timer / transitionDuration);

            transitionImage.rectTransform.localScale = Vector3.Lerp(targetScale, initialScale, progresStart);
            if(timer >= 1)
            {
                isEnding = false;
                if(NodeManager.instance)
                    NodeManager.instance.CompleteQuest();

                SceneManager.LoadScene("HUB");
            }

        }
    }

    public void InitLevelTransition()
    {
        isStarting = true;
    }

    public void EndLevelTransition()
    {
        if(!isEnding)
            timer = 0f;
        isEnding = true;
    }

}
