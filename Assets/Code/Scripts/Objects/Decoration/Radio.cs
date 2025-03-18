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

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if (!_objectHolder.GetHasObjectPicked())
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "grab"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };
        else if (_objectHolder.GetHandInteractableObject() == this)
        {
            if (!isPlaying)
                return new HintController.Hint[]
                {
                    new HintController.Hint(HintController.ActionType.INTERACT, "drop"),
                    new HintController.Hint(HintController.ActionType.USE, "play_music")
                };
            else
                return new HintController.Hint[]
                {
                    new HintController.Hint(HintController.ActionType.INTERACT, "drop"),
                    new HintController.Hint(HintController.ActionType.USE, "stop_music")
                };


        }
        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        };
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        base.Interact(_objectHolder); 
        _objectHolder.hintController.UpdateActionType(ShowNeededInputHint(_objectHolder));

    }

    public override void Use(ObjectHolder _objectHolder)
    {
        if (isPlaying)
            StopPlaying();
        else
            Play();

        _objectHolder.hintController.UpdateActionType(ShowNeededInputHint(_objectHolder));
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
