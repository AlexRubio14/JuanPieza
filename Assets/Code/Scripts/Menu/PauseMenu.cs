using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    [SerializeField] private Button resumeButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseManager.Instance.TogglePause();
            resumeButton.Select();
            pauseMenuUI.SetActive(PauseManager.Instance.IsPaused);
        }
    }

    public void OnResumeButton()
    {
        PauseManager.Instance.ResumeGame();
        pauseMenuUI.SetActive(PauseManager.Instance.IsPaused);
    }

    public void OnHubButton()
    {
        PauseManager.Instance.ResumeGame();
        pauseMenuUI.SetActive(PauseManager.Instance.IsPaused);
        SceneManager.LoadScene("Hub");
    }
    
    public void OnMenuButton()
    {
        foreach (var player in PlayersManager.instance.players)
        {
            Destroy(player.playerInput.gameObject);
        }
        Destroy(PlayersManager.instance.gameObject);
        Destroy(gameObject);
        
        PauseManager.Instance.ResumeGame();
        pauseMenuUI.SetActive(PauseManager.Instance.IsPaused);
        SceneManager.LoadScene("MainMenu");
    }
}
