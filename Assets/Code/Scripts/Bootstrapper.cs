using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;

public class Bootstrapper : MonoBehaviour
{
    private const string BOOTSTRAP_SCENE_NAME = "Bootstrapper";
    private const string MAINSCENE_SCENE_NAME = "MainMenu";

    async void Start()
    {
        Application.runInBackground = true;

        if (!UnityServices.State.Equals(ServicesInitializationState.Initialized))
        {
            await UnityServices.InitializeAsync();
        }

        if (SceneManager.loadedSceneCount == 1)
        {
            SceneManager.LoadScene(MAINSCENE_SCENE_NAME, LoadSceneMode.Additive);
        }
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
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