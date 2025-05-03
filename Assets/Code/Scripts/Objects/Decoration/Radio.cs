using System.Collections.Generic;
using UnityEngine;

public class Radio : Resource, ICatapultAmmo
{
    [Space, Header("Radio"), SerializeField] private List<AudioClip> musicsList;
    private List<float> musicsPlayed = new List<float>();
    [SerializeField]
    private AudioClip radioClip;

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        AudioManager.instance.ApplyRadioFilter(AudioManager.instance.radioAs);
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
        AudioManager.instance.Play2dOneShotSound(radioClip, "Objects", 0.5f, 0.6f, 0.8f);
    }

    private void StopPlaying()
    {
        AudioManager.instance.musicAs.UnPause();
        AudioManager.instance.radioAs.clip = null;
        AudioManager.instance.radioAs.Stop();
        AudioManager.instance.Play2dOneShotSound(radioClip, "Objects", 0.5f, 1.1f, 1.3f);

    }
    
    public void RadioFallAtWater()
    {
        AudioManager.instance.ApplyUnderwaterFilter(AudioManager.instance.radioAs);
    }

}
