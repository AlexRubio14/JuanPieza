using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPlayerController : MonoBehaviour
{
    private const string BOOTSTRAP_SCENE_NAME = "Bootstrapper";

    public static TestPlayerController Instance;
    [HideInInspector]
    public string lastSceneActive;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            return;
        }

        if (PlayersManager.instance != null)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        if (SceneManager.GetActiveScene().name != BOOTSTRAP_SCENE_NAME && SceneManager.GetActiveScene().name != "")
        {
            lastSceneActive = SceneManager.GetActiveScene().name;
            //Debug.Log("Last Scene Active: " + lastSceneActive);
        }
        if (SceneManager.GetActiveScene().name != "PlayerSelector")
        {
            SceneManager.LoadScene("PlayerSelector");
        }
    }

}
