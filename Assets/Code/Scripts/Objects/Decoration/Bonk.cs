using UnityEngine;

public class Bonk : RepairObject
{
    private Animation animationController;
    [SerializeField] private AudioClip bonkClip;
    [SerializeField] private RumbleController.RumblePressets rumble;
    protected override void Awake()
    {
        base.Awake();
        animationController = GetComponent<Animation>();
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        if (!CanInteract(_objectHolder) || state.GetIsBroken())
            return;

        //Bong
        animationController.Stop();
        animationController.Play();

        //Sonido
        AudioManager.instance.Play2dOneShotSound(bonkClip, "Objects", 0.2f, 0.9f, 1.1f);

        //Vibracion
        foreach (PlayersManager.PlayerData item in PlayersManager.instance.players)
            item.rumbleController.AddRumble(rumble);    

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
    
}
