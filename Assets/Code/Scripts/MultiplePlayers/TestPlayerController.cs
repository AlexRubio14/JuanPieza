using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPlayerController : MonoBehaviour
{
    public static TestPlayerController Instance;
    [HideInInspector]
    public string lastSceneActive;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);

        if (PlayersManager.instance != null)
        {
            Destroy(this);
            return;
        }


        Instance = this;

        lastSceneActive = SceneManager.GetActiveScene().name;

        DontDestroyOnLoad(gameObject);
        
        SceneManager.LoadScene("PlayerSelector");
    }
}
