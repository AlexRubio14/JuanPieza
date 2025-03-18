using UnityEngine;

public class FinalSceneController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip finalMusicCineClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.StopLoopSound(AudioManager.instance.musicAs);
        audioSource =  AudioManager.instance.Play2dLoop(finalMusicCineClip, "Music", 1, 1, 1);
    }
}
