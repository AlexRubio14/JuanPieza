using System.Collections.Generic;
using UnityEngine;

public class Radio : Resource, ICatapultAmmo
{
    [Space, Header("Radio"), SerializeField] private List<AudioClip> musicsList;
    private AudioSource audioSource;

    private bool isPlaying;

    protected override void Awake()
    {
        base.Awake();
        isPlaying = false;
    }

    public override void Interact(ObjectHolder _objectHolder) { }
    public override void Use(ObjectHolder _objectHolder) 
    {
        if (isPlaying)
            StopPlaying();
        else
            Play();
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return false;
    }
 
    private void Play()
    {
        int randomMusic = Random.Range(0, musicsList.Count);

        AudioManager.instance.musicAs.Pause();
        audioSource = AudioManager.instance.Play2dLoop(musicsList[randomMusic], "Radio", 1, 1, 1);
        isPlaying = true;
    }
    private void StopPlaying()
    {
        AudioManager.instance.musicAs.UnPause();
        audioSource.clip = null;
        audioSource.Stop();
        isPlaying = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (isPlaying)
            StopPlaying();
    }
}
