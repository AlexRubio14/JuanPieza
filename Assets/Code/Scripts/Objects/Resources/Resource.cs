using UnityEngine;

public abstract class Resource : InteractableObject
{

    [SerializeField] protected AudioClip dropItemClip;
    public override void Interact(ObjectHolder _objectHolder)
    {
        if(!_objectHolder.GetHasObjectPicked())
        {
            PickItem(_objectHolder);
            return;
        }

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
    }
    private void DropItem(ObjectHolder _objectHolder)
    {
        _objectHolder.RemoveItemFromHand();
        AudioManager.instance.Play2dOneShotSound(dropItemClip, "Objects");
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return !handObject;
    }

    public override HintController.ActionType ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        if (!handObject)
        {
            return HintController.ActionType.INTERACT;
        }
        return HintController.ActionType.NONE;
    }
}
