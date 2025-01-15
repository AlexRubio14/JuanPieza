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
        AudioManager.instance.Play2dOneShotSound(bonkClip, "Objects",1,0.9f,1.1f);
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
}
