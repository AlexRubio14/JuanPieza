using UnityEngine;

public abstract class Resource : InteractableObject
{

    [SerializeField] protected AudioClip dropItemClip;

    public override void Grab(ObjectHolder _objectHolder)
    {
        if (!_objectHolder.GetHasObjectPicked())
            PickItem(_objectHolder);
    }
    public override void Release(ObjectHolder _objectHolder)
    {
        InteractableObject nearObj = _objectHolder.GetNearestInteractableObject();

        if (nearObj && nearObj is Box && nearObj.CanInteract(_objectHolder))
        {
            (nearObj as Box).AddItemInBox();
            InteractableObject currentObject = _objectHolder.RemoveItemFromHand();
            Destroy(currentObject.gameObject);
        }
        else
            DropItem(_objectHolder);
    }
    private void PickItem(ObjectHolder _objectHolder)
    {
        _objectHolder.ChangeObjectInHand(this);

        selectedVisual.Hide();

        AudioManager.instance.Play2dOneShotSound(_objectHolder.pickUpClip, "Objects");

        _objectHolder.hintController.UpdateActionType(ShowNeededInputHint(_objectHolder));

        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", true);
    }
    private void DropItem(ObjectHolder _objectHolder)
    {
        _objectHolder.RemoveItemFromHand();
        AudioManager.instance.Play2dOneShotSound(dropItemClip, "Objects");
        _objectHolder.hintController.UpdateActionType(ShowNeededInputHint(_objectHolder));
        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", false);

    }

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        if (!handObject)
        {
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "grab"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")

            };
        }
        else if (handObject == this)
        {
            return new HintController.Hint[] 
            { 
                new HintController.Hint(HintController.ActionType.INTERACT, "drop"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "") 
            };
        }

        return new HintController.Hint[] 
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        };
    }
}
