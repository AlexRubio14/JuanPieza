using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("Battle0");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
