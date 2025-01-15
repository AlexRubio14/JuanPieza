using UnityEngine;

public class Box : RepairObject
{
    [Space, Header("Item"), SerializeField] protected ObjectSO itemDropped;
    [SerializeField] protected int itemsInBox;
    [SerializeField] protected AudioClip dropItemClip;
    public virtual void AddItemInBox()
    {
        itemsInBox++;
        ShipsManager.instance.playerShip.AddWeight(itemDropped.weight);
        AudioManager.instance.Play2dOneShotSound(dropItemClip, "Objects");
    }
    public virtual void RemoveItemInBox()
    {
        itemsInBox--;
        ShipsManager.instance.playerShip.RemoveWeight(itemDropped.weight);
    }
    public int GetItemsInBox()
    {
        return itemsInBox;
    }
    public ObjectSO GetItemToDrop()
    {
        return itemDropped;
    }

    public bool HasItems()
    {
        return itemsInBox > 0;
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        base.Interact(_objectHolder);

        if (!CanInteract(_objectHolder) || state.GetIsBroken())
            return;

        if (!_objectHolder.GetHasObjectPicked())
        {
            RemoveItemInBox();
            InteractableObject boxObject = _objectHolder.InstantiateItemInHand(itemDropped);
            ShipsManager.instance.playerShip.AddInteractuableObject(boxObject);
            _objectHolder.ChangeObjectInHand(boxObject);

        }
        else if (_objectHolder.GetHasObjectPicked())
        {
            AddItemInBox();
            InteractableObject currentObject = _objectHolder.RemoveItemFromHand();
            Destroy(currentObject.gameObject);
        }

    }

    public override void UseItem(ObjectHolder _objectHolder) { }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken())
           return base.CanInteract(_objectHolder);

        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return !handObject && HasItems()
            || handObject && handObject.objectSO == objectToInteract;
    }

    public override HintController.ActionType ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken())
            return base.ShowNeededInputHint(_objectHolder);
        
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
    
        if (!handObject && HasItems() || handObject && handObject.objectSO == objectToInteract)
            return HintController.ActionType.INTERACT;
        
        return HintController.ActionType.NONE;
    }

    public ObjectSO GetItemDrop()
    {
        return itemDropped;
    }
}
