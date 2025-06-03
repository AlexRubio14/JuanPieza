using System;
using System.Collections;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    public enum Language { SPANISH, ENGLISH, CATALAN }
    public Language language {  get; private set; }

    public Action<Language> OnLanguageChange;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        language = Language.SPANISH;
    }

    public void ChangeLanguage(Language _language)
    {
        language = _language;

        if(OnLanguageChange != null)
            OnLanguageChange(language);
    }
}
