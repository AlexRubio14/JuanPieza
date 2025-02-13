using System.Collections.Generic;
using UnityEngine;

public class Radio : Resource
{
    [SerializeField] private List<AudioClip> musicsList;
    [SerializeField] public AudioSource audioSource;

    private bool isPlaying;

    protected override void Awake()
    {
        base.Awake();
        isPlaying = false;

        audioSource.loop = true;
        audioSource.pitch = 1;
        audioSource.volume = 1;
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return !_objectHolder.GetHasObjectPicked();
    }

    public override HintController.ActionType ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if (!_objectHolder.GetHasObjectPicked())
            return HintController.ActionType.INTERACT;
        else if (_objectHolder.GetHandInteractableObject() == this)
            return HintController.ActionType.USE;

        return HintController.ActionType.NONE;
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        base.Interact(_objectHolder);
        _objectHolder.hintController.UpdateActionType(HintController.ActionType.USE);
    }

    public override void Use(ObjectHolder _objectHolder)
    {
        //playear animacion player
        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", false);

        _objectHolder.hintController.UpdateActionType(HintController.ActionType.NONE);

        if (isPlaying)
            StopPlaying();
        else
            Play();
    }

    private void Play()
    {
        int randomMusic = Random.Range(0, musicsList.Count);

        AudioManager.instance.musicAs.Pause();
        audioSource = AudioManager.instance.Play2dLoop(musicsList[randomMusic], "Music", 1, 1, 1);
        isPlaying = true;
    }

    private void StopPlaying()
    {
        AudioManager.instance.musicAs.UnPause();
        audioSource.clip = null; 
        audioSource.Stop();
        isPlaying = false;
    }

    private void OnDestroy()
    {
        if (isPlaying)
            StopPlaying();
    }
}
