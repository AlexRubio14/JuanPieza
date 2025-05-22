using UnityEngine;

public class FinalSceneController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip finalMusicCineClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.radioAs.Stop();
        AudioManager.instance.musicAs.Stop(); 
        AudioManager.instance.musicAs.clip = finalMusicCineClip;
        AudioManager.instance.musicAs.Play();
    }
}
