using UnityEngine;

public class BartenderNPC : InteractNPC
{
    [Space, Header("Item"), SerializeField] protected ObjectSO itemDropped;
    [SerializeField]
    private AudioClip serveDrinkClip;
    protected override void Start()
    {
        base.Start();
    }


    public override void Grab(ObjectHolder _objectHolder)
    {
        base.Interact((_objectHolder));
        InteractableObject boxObject = _objectHolder.InstantiateItemInHand(itemDropped);
        _objectHolder.playerController.animator.SetBool("Pick", true);
        AudioManager.instance.Play2dOneShotSound(serveDrinkClip, "Objects", 0.4f, 0.95f, 1.05f);
    }

    public override void Release(ObjectHolder _objectHolder)
    {

    }

    public override void Interact(ObjectHolder _objectHolder)
    {

    }

    public override void Use(ObjectHolder _objectHolder)
    {

    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return !handObject && base.CanInteract(_objectHolder);
    }
}
