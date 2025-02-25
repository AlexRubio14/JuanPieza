using System.Collections.Generic;
using UnityEngine;

public class Radio : Resource
{
    [Space, Header("Radio"), SerializeField] private List<AudioClip> musicsList;
    private AudioSource audioSource;

    private bool isPlaying;

    protected override void Awake()
    {
        base.Awake();
        isPlaying = false;
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return !_objectHolder.GetHasObjectPicked();
    }

    public override HintController.ActionType[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if (!_objectHolder.GetHasObjectPicked())
            return new HintController.ActionType[] { HintController.ActionType.INTERACT, HintController.ActionType.CANT_USE};
        else if (_objectHolder.GetHandInteractableObject() == this)
            return new HintController.ActionType[] { HintController.ActionType.INTERACT, HintController.ActionType.USE };

        return new HintController.ActionType[] { HintController.ActionType.NONE };
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        base.Interact(_objectHolder); 
        _objectHolder.hintController.UpdateActionType(new HintController.ActionType[] { HintController.ActionType.USE });

    }

    public override void Use(ObjectHolder _objectHolder)
    {
        //playear animacion player
        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", false);

        _objectHolder.hintController.UpdateActionType( new HintController.ActionType[] { HintController.ActionType.NONE } );

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

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (isPlaying)
            StopPlaying();
    }
}
