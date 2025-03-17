using UnityEngine;
using UnityEngine.UI;

public class AcceptButton : MonoBehaviour
{
    private Button acceptButton;
    private ImageFloatEffect floatEffect;

    private void Awake()
    {
        acceptButton = GetComponent<Button>();

        acceptButton.onClick.AddListener(OnButtonClick);

        floatEffect = GetComponent<ImageFloatEffect>();
    }

    private void OnEnable()
    {
        acceptButton.Select();

        if (floatEffect)
            floatEffect.canFloat = true;
    }

    void OnButtonClick()
    {
        QuestManager.Instance.AcceptQuest();
    }
}
