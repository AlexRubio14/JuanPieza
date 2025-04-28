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

    [Space, SerializeField]
    private GameObject menuObject;

    [SerializeField]
    private Button settingsStartSelectedButton;
    [SerializeField]
    private Button menuSettingsButton;

    private void Start()
    {
        UpdateMusicVolume();
        UpdateSfxVolume();
    }
    public void UpdateMusicVolume()
    {
        float volume = (musicSlider.value / 10) + 0.0001f;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void UpdateSfxVolume()  
    {
        float volume = (sfxSlider.value / 10) + 0.0001f;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }


    public void OpenSettings()
    {
        menuObject.SetActive(false);
        gameObject.SetActive(true);
        settingsStartSelectedButton.Select();
    }

    public void CloseSettings()
    {
        menuObject.SetActive(true);
        gameObject.SetActive(false);
        menuSettingsButton.Select();
    }
}
