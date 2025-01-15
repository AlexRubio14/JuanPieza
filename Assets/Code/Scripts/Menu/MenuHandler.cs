using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{

    private void Start()
    {
        AudioManager.instance.musicAs = AudioManager.instance.Play2dLoop(AudioManager.instance.musicClip, "Music", 1, 1, 1);
    }

    public void PlayButton()
    {
        AudioManager.instance.seagullAs = AudioManager.instance.Play2dLoop(AudioManager.instance.seagullClip, "Music", 1, 1, 1);
        SceneManager.LoadScene("Battle0");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
