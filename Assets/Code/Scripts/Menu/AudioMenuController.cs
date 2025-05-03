using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMenuController : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    private string MUSIC_VALUE_KEY = "MUSIC_VALUE";
    private string SFX_VALUE_KEY = "SFX_VALUE";

    private void Start()
    {
        if (PlayerPrefs.HasKey(MUSIC_VALUE_KEY))
            musicSlider.value = PlayerPrefs.GetInt(MUSIC_VALUE_KEY);
        else
            PlayerPrefs.SetInt(MUSIC_VALUE_KEY, (int)musicSlider.value);

        if (PlayerPrefs.HasKey(SFX_VALUE_KEY))
            sfxSlider.value = PlayerPrefs.GetInt(SFX_VALUE_KEY);
        else 
            PlayerPrefs.SetInt(SFX_VALUE_KEY, (int)sfxSlider.value);

        UpdateMusicVolume();
        UpdateSfxVolume();
    }

    public void UpdateMusicVolume()
    {
        float volume = (musicSlider.value / 10) + 0.0001f;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetInt(MUSIC_VALUE_KEY, (int)musicSlider.value);
    }
    public void UpdateSfxVolume()  
    {
        float volume = (sfxSlider.value / 10) + 0.0001f;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetInt(SFX_VALUE_KEY, (int)sfxSlider.value);

    }
}
