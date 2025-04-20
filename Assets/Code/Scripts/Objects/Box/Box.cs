using UnityEngine;

public class Box : RepairObject
{
    [Space, Header("Item"), SerializeField] protected ObjectSO itemDropped;
    [SerializeField] protected int itemsInBox;
    [SerializeField] protected AudioClip dropItemClip;
    
    public override void Grab(ObjectHolder _objectHolder)
    {
        if (!_objectHolder.GetHasObjectPicked())
        {
            RemoveItemInBox();
            InteractableObject boxObject = _objectHolder.InstantiateItemInHand(itemDropped);
            ShipsManager.instance.playerShip.AddInteractuableObject(boxObject);
            _objectHolder.ChangeObjectInHand(boxObject);
            _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", true);

        }
    }
    public override void Interact(ObjectHolder _objectHolder)
    {
        if (!CanInteract(_objectHolder) || state.GetIsBroken())
            return;
    }
    public override void Use(ObjectHolder _objectHolder) { }

    public override bool CanGrab(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return !handObject && HasItems(); //Si no tengo objetos en las manos y la caja tiene items
    }
    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken())
           return base.CanInteract(_objectHolder);

        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
        return handObject && handObject.objectSO == objectToInteract; //Si tengo un objeto en la mano y es del mismo tipo que el que dropea
    }

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if (state.GetIsBroken())
            return base.ShowNeededInputHint(_objectHolder);
        
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
    
        if (!handObject && HasItems())
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "grab"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };
        else if (handObject && handObject.objectSO == objectToInteract)
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "store"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };

        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        };
    }

    public virtual void AddItemInBox(bool _makeSound = true, int cuantity = 1)
    {
        if (cuantity == 1)
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
    public ObjectSO GetItemDrop()
    {
        return itemDropped;
    }
}
