using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Slider audioSlider;

    private Animator _anim;

    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _anim.SetBool("OpenMainMenu", true);
        _anim.SetBool("OpenOptionsMenu", false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Hub");
    }
    
    public void OpenOptions()
    {
        _anim.SetBool("OpenMainMenu", false);
        _anim.SetBool("OpenOptionsMenu", true);
    }

    public void CloseOptions()
    {
        _anim.SetBool("OpenOptionsMenu", false);
        _anim.SetBool("OpenMainMenu", true);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
    public void SelectOptionsButton()
    {
        optionsButton.Select();
    }
    public void SelectAudioSlider()
    {
        audioSlider.Select();
    }
}
