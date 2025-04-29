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
}
