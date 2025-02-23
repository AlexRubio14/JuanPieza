using UnityEngine;

public class Box : RepairObject
{
    [Space, Header("Item"), SerializeField] protected ObjectSO itemDropped;
    [SerializeField] protected int itemsInBox;
    [SerializeField] protected AudioClip dropItemClip;
    public virtual void AddItemInBox(bool _makeSound = true, int cuantity = 1)
    {
        if(cuantity == 1)
            itemsInBox++;
        else
            itemsInBox = cuantity;
        if (_makeSound)
            AudioManager.instance.Play2dOneShotSound(dropItemClip, "Objects");
    }
    public virtual void RemoveItemInBox()
    {
        itemsInBox--;
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

    }

    public override void Use(ObjectHolder _objectHolder) { }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken())
           return base.CanInteract(_objectHolder);

        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return !handObject && HasItems() //Si no tengo objetos en las manos y la caja tiene items
            || handObject && handObject.objectSO == objectToInteract; //Si tengo un objeto en la mano y es del mismo tipo que el que dropea
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
