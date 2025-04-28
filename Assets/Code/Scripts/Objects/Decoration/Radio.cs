using System.Collections.Generic;
using UnityEngine;

public class Radio : Resource, ICatapultAmmo
{
    [Space, Header("Radio"), SerializeField] private List<AudioClip> musicsList;
    private List<float> musicsPlayed = new List<float>();

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Interact(ObjectHolder _objectHolder) { }

    public override void Use(ObjectHolder _objectHolder) 
    {
        if (AudioManager.instance.radioAs.isPlaying)
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
        int randomMusic;

        do
        {
            randomMusic = Random.Range(0, musicsList.Count);
        } 
        while (musicsPlayed.Contains(randomMusic));
        musicsPlayed.Add(randomMusic);
        if (musicsPlayed.Count > 3)
        {
            musicsPlayed.RemoveAt(0);
        }

        AudioManager.instance.musicAs.Pause();
        AudioManager.instance.PlayLoopSound(AudioManager.instance.radioAs, musicsList[randomMusic], "Radio", 1f, 1f, 1f);
    }

    private void StopPlaying()
    {
        AudioManager.instance.musicAs.UnPause();
        AudioManager.instance.radioAs.clip = null;
        AudioManager.instance.radioAs.Stop();
    }    
}
