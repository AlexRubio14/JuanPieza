using UnityEngine;

public class Bonk : RepairObject
{
    private Animation animationController;
    [SerializeField] private AudioClip bonkClip;

    protected override void Awake()
    {
        base.Awake();
        animationController = GetComponent<Animation>();
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        base.Interact(_objectHolder);

        if (!CanInteract(_objectHolder) || state.GetIsBroken())
            return;

        //Bong
        animationController.Stop();
        animationController.Play();

        //Sonido
        AudioManager.instance.Play2dOneShotSound(bonkClip, "Objects", 0.2f, 0.9f, 1.1f);
    }

    public override bool CanGrab(ObjectHolder _objectHolder)
    {
        return false;
    }
    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken())
            return base.CanInteract(_objectHolder);

        return !_objectHolder.GetHandInteractableObject();
    }
    
    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken())
            return base.ShowNeededInputHint(_objectHolder);

        if (!_objectHolder.GetHandInteractableObject())
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "touch_bonk"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")

            };

        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")

        };
    }
}
