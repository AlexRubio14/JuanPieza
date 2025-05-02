using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    public bool IsPaused { get; private set; }
    
    public GameObject pauseMenuUI;
    [SerializeField] private Button resumeButton;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        
        if (IsPaused)
            PauseGame();
        else
            ResumeGame();
        
        pauseMenuUI.SetActive(IsPaused);
    }

    public void PauseGame()
    {
        resumeButton.Select();
        IsPaused = true;
        Time.timeScale = 0f;
        foreach (PlayersManager.PlayerData item in PlayersManager.instance.players)
        {
            if (PlayersManager.instance)
                item.playerInput.SwitchCurrentActionMap("MapMenu");
        }
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        foreach (PlayersManager.PlayerData item in PlayersManager.instance.players)
        {
            if (item.playerInput.inputIsActive)
            {
                item.playerInput.SwitchCurrentActionMap("Gameplay");
            }
        }
    }
    
    public void OnResumeButton()
    {
        ResumeGame();
        pauseMenuUI.SetActive(IsPaused);
    }

    public void OnHubButton()
    {
        ResumeGame();
        pauseMenuUI.SetActive(IsPaused);
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
        
        ResumeGame();
        pauseMenuUI.SetActive(IsPaused);
        if (NodeManager.instance)
            Destroy(NodeManager.instance);
        SceneManager.LoadScene("MainMenu");
    }
}
