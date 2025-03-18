using UnityEngine;

public class Beer : Resource
{

    public override void Interact(ObjectHolder _objectHolder)
    {
        base.Interact(_objectHolder);
        _objectHolder.hintController.UpdateActionType(ShowNeededInputHint(_objectHolder));
    }
    public override void Use(ObjectHolder _objectHolder)
    {
        //playear animacion player

        _objectHolder.RemoveItemFromHand();
        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", false);
        Destroy(gameObject);
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
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "drop"),
                new HintController.Hint(HintController.ActionType.USE, "drink")
            };

        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        };
    }
}
