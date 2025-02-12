using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Radio : RepairObject
{
    [SerializeField] private List<AudioClip> musicsList;
    [SerializeField] public AudioSource audioSource;

    private bool isPlaying;

    protected override void Awake()
    {
        base.Awake();
        isPlaying = false;

        //audioSource.outputAudioMixerGroup = ;
        audioSource.loop = true;
        audioSource.pitch = 1;
        audioSource.volume = 1;
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        base.Interact(_objectHolder);

        if (!CanInteract(_objectHolder) || state.GetIsBroken())
            return;

        //Sonido
        if (isPlaying)
            StopPlaying();
        else
            Play();
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken())
            return base.CanInteract(_objectHolder);


        return !_objectHolder.GetHandInteractableObject();
    }
    
    public override HintController.ActionType ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken())
            return base.ShowNeededInputHint(_objectHolder);

        if (!_objectHolder.GetHandInteractableObject())
            return HintController.ActionType.INTERACT;


        return HintController.ActionType.NONE;
    }

    private void Play()
    {
        int randomMusic = Random.Range(0, musicsList.Count);
        Debug.Log(randomMusic);
        Debug.Log(musicsList.Count);
        audioSource.clip = musicsList[randomMusic];
        audioSource.Play();
    }
    private void StopPlaying()
    {
        audioSource.clip = null; 
        audioSource.Stop();
    }
}
