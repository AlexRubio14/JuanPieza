using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radio : Resource, ICatapultAmmo
{
    [Space, Header("Radio"), SerializeField] private List<AudioClip> musicsList;
    private List<float> musicsPlayed = new List<float>();
    [field: SerializeField]
    public float timeToBreakRadio {  get; private set; }
    [SerializeField]
    private AudioClip toggleRadioClip;
    [field: SerializeField]
    public AudioClip breakRadioClip { get; private set; }
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
            StopPlaying(toggleRadioClip);
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
        AudioManager.instance.Play2dOneShotSound(toggleRadioClip, "Objects", 0.5f, 0.6f, 0.8f);
    }

    public static void StopPlaying(AudioClip _buttonClip)
    {
        AudioManager.instance.musicAs.UnPause();
        AudioManager.instance.radioAs.clip = null;
        AudioManager.instance.radioAs.Stop();
        if(_buttonClip)
            AudioManager.instance.Play2dOneShotSound(_buttonClip, "Objects", 0.5f, 1.1f, 1.3f);
    }
    public static void BreakRadio(AudioClip _breakClip)
    {
        AudioManager.instance.Play2dOneShotSound(_breakClip, "Objects", 1, 0.95f, 1.05f);
        StopPlaying(null);
    }

    public static IEnumerator PlayMainMusic()
    {
        AudioManager.instance.musicAs.Pause();
        yield return new WaitForSeconds(1);
        AudioManager.instance.musicAs.UnPause();
    }
    public void RadioFallAtWater()
    {
        AudioManager.instance.ApplyUnderwaterFilter(AudioManager.instance.radioAs);
    }

}
