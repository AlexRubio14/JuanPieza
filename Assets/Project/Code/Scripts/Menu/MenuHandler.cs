using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{

    private void Start()
    {
        AudioManager.instance.musicAs.volume = 0.2f;
        AudioManager.instance.musicAs.outputAudioMixerGroup = AudioManager.instance.GetMixer().FindMatchingGroups("Game Theme")[0];
        AudioManager.instance.musicAs.loop = true;
        AudioManager.instance.musicAs.pitch = 1;
        AudioManager.instance.musicAs.clip = AudioManager.instance.musicClip;
        AudioManager.instance.musicAs.Play();
    }

    public void PlayButton()
    {
        AudioManager.instance.seagullAs = AudioManager.instance.Play2dLoop(AudioManager.instance.seagullClip, "Ambient", 1.5f, 1, 1);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene("Hub");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
