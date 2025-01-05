using TMPro.EditorUtilities;
using UnityEngine;

public class Box : InteractableObject
{
    [Space, Header("Item"), SerializeField] protected ObjectSO itemDropped;
    [SerializeField] protected int itemsInBox;
    public virtual void AddItemInBox()
    {
        itemsInBox++;
    }

    public virtual void RemoveItemInBox()
    {
        itemsInBox--;
    }

    public bool HasItem()
    {
        return itemsInBox > 0;
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        if (!CanInteract(_objectHolder))
            return;

        if (!_objectHolder.GetHasObjectPicked())
        {
            RemoveItemInBox();
            _objectHolder.InstantiateItemInHand(itemDropped);
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
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return !handObject && HasItem() 
            || handObject && handObject.objectSO == objectToInteract;
    }
}
