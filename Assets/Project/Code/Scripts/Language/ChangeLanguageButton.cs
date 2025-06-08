using UnityEngine;

public class ChangeLanguageButton : MonoBehaviour
{
    [SerializeField]
    private LanguageManager.Language language;

    public void ChangeLanguage()
    {
        LanguageManager.instance.ChangeLanguage(language);
    }
}
