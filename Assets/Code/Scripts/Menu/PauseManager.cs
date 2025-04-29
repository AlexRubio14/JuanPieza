using System;
using UnityEngine;
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
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }
    
    public void OnResumeButton()
    {
        ResumeGame();
        pauseMenuUI.SetActive(PauseManager.Instance.IsPaused);
    }

    public void OnHubButton()
    {
        ResumeGame();
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
