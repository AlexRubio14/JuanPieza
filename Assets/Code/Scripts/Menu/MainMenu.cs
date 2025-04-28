using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] protected Button newGameButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Slider audioSlider;

    private Animator _anim;

    private Button lastButton;

    private void Awake()
    {
        _anim = GetComponent<Animator>();

    }

    private void Start()
    {
        _anim.SetBool("OpenMainMenu", true);
        _anim.SetBool("OpenOptionsMenu", false);

        AudioManager.instance.musicAs.volume = 0.2f;
        AudioManager.instance.musicAs.outputAudioMixerGroup = AudioManager.instance.GetMixer().FindMatchingGroups("Game Theme")[0];
        AudioManager.instance.musicAs.loop = true;
        AudioManager.instance.musicAs.pitch = 1;
        AudioManager.instance.musicAs.clip = AudioManager.instance.musicClip;
        AudioManager.instance.musicAs.Play();

        AudioManager.instance.seagullAs = AudioManager.instance.Play2dLoop(AudioManager.instance.seagullClip, "Ambient", 1.5f, 1, 1);

        if (!PlayerPrefs.HasKey("Hub_Tutorial"))
        {
            playButton.interactable = false;
            Navigation newNav = newGameButton.navigation;

            newNav.selectOnUp = exitButton;
            newGameButton.navigation = newNav;

            newNav = exitButton.navigation;
            newNav.selectOnDown = newGameButton;
            exitButton.navigation = newNav;
        }
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene("Hub");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Hub");
    }
    
    public void OpenOptions()
    {
        _anim.SetBool("OpenMainMenu", false);
        _anim.SetBool("OpenOptionsMenu", true);
        lastButton = optionsButton;
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
    public void SelectLastButtonButton()
    {
        if (lastButton)
            lastButton.Select();
        else if (playButton.interactable)
            playButton.Select();
        else
            newGameButton.Select();        
    }
    public void SelectAudioSlider()
    {
        audioSlider.Select();
    }
}
