using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    private const string BOOTSTRAP_SCENE_NAME = "Bootstrapper";
    private const string MAINSCENE_SCENE_NAME = "MainMenu";

    private void Start()
    {
        Application.runInBackground = true;
        
        if (SceneManager.loadedSceneCount == 1)
        {
            SceneManager.LoadScene(MAINSCENE_SCENE_NAME, LoadSceneMode.Additive);
        }
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {

        if (TestPlayerController.Instance.cancelBootstrap)
            return;
#if UNITY_EDITOR
        Scene currentLoadedEditorScene = SceneManager.GetActiveScene();
#endif

        if (!SceneManager.GetSceneByName(BOOTSTRAP_SCENE_NAME).isLoaded)
        {
            SceneManager.LoadScene(BOOTSTRAP_SCENE_NAME);
        }

#if UNITY_EDITOR
        if (currentLoadedEditorScene.IsValid())
        {
            SceneManager.LoadSceneAsync(currentLoadedEditorScene.name, LoadSceneMode.Additive);
        }
#endif
    }
}