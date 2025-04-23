using UnityEngine;

public class BartenderNPC : InteractNPC
{
    [Space, Header("Item"), SerializeField] protected ObjectSO itemDropped;
    protected override void Start()
    {
        base.Start();
    }


    public override void Grab(ObjectHolder _objectHolder)
    {
        base.Interact((_objectHolder));
        InteractableObject boxObject = _objectHolder.InstantiateItemInHand(itemDropped);
        _objectHolder.ChangeObjectInHand(boxObject);
        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", true);
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

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        return new HintController.Hint[]
          {
                new HintController.Hint(HintController.ActionType.INTERACT, "grab"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
          };
    }
}
