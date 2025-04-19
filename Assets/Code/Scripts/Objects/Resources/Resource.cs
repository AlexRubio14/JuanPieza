using UnityEngine;

public abstract class Resource : InteractableObject
{

    [SerializeField] protected AudioClip dropItemClip;
    public override void Interact(ObjectHolder _objectHolder)
    {
        InteractableObject nearObj = _objectHolder.GetNearestInteractableObject();

        if (nearObj && nearObj is Box && nearObj.CanInteract(_objectHolder))
        {
            (nearObj as Box).AddItemInBox();
            InteractableObject currentObject = _objectHolder.RemoveItemFromHand();
            Destroy(currentObject.gameObject);
        }
    }

    public override void Use(ObjectHolder _objectHolder)
    {
        Throw();
    }

  

    public override void Grab(ObjectHolder _objectHolder)
    {
        if (!_objectHolder.GetHasObjectPicked())
        {
            PickItem(_objectHolder);
            return;
        }
    }

    public override void Release(ObjectHolder _objectHolder)
    {
        
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

    public void Throw()
    {

    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return !handObject;
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
