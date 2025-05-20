using UnityEngine;
using UnityEngine.UI;

public class BackButtonController : MonoBehaviour
{
    private Button backButton;

    private void Awake()
    {
        backButton = GetComponent<Button>();
    }

    public void SetBackButton()
    {
        if (MenuBackController.instance)
            MenuBackController.instance.backButton = backButton;
    }
    private void OnEnable()
    {
        SetBackButton();
    }

}
