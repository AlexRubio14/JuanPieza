using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{

    private void Start()
    {
        AudioManager.instance.musicAs.clip = AudioManager.instance.musicClip;
        AudioManager.instance.musicAs.volume = 0.2f;
        AudioManager.instance.musicAs.Play();

    }

    public void PlayButton()
    {
        AudioManager.instance.seagullAs = AudioManager.instance.Play2dLoop(AudioManager.instance.seagullClip, "Music", 1, 1, 1);
        SceneManager.LoadScene("Hub");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
