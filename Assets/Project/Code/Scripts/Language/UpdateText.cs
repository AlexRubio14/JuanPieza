using UnityEngine;
using AYellowpaper.SerializedCollections;
using TMPro;

public class UpdateText : MonoBehaviour
{
    private TextMeshProUGUI text;
    public SerializedDictionary<LanguageManager.Language, string> languageTexts;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        LanguageManager.instance.OnLanguageChange += UpdateTextByLanguage;
        UpdateTextByLanguage(LanguageManager.instance.language);
    }
    private void OnDestroy()
    {
        LanguageManager.instance.OnLanguageChange -= UpdateTextByLanguage;
    }

    private void UpdateTextByLanguage(LanguageManager.Language _language)
    {
        text.text = languageTexts[_language];
    }
}
