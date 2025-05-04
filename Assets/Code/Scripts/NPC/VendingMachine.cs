using UnityEngine;

public class VendingMachine : InteractableObject
{
    
    [Space, Header("Item"), SerializeField] protected ObjectSO itemDropped;
    [SerializeField]
    private AudioClip vendingClip;
    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return false;
    }

    public override void Grab(ObjectHolder _objectHolder)
    {
        InteractableObject boxObject = _objectHolder.InstantiateItemInHand(itemDropped);
        _objectHolder.playerController.animator.SetBool("Pick", true);
        AudioManager.instance.Play2dOneShotSound(vendingClip, "Objects", 0.5f, 0.9f, 1.1f);
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        throw new System.NotImplementedException();
    }

    public override void Release(ObjectHolder _objectHolder)
    {
        throw new System.NotImplementedException();
    }

    public override void Use(ObjectHolder _objectHolder)
    {
        throw new System.NotImplementedException();
    }
}
