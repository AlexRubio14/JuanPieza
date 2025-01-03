using System.Collections.Generic;
using UnityEngine;

public class Box : InteractableObject
{
    [Header("Item")]
    [SerializeField] private InteractableObject itemDropped;
    [SerializeField] private Collider itemDroppedCollider;
    protected int itemsInBox;
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
        if (!_objectHolder.GetHasObjectPicked() && HasItem())
        {
            RemoveItemInBox();
            _objectHolder.SetHasObjectPicked(true);
            _objectHolder.InstantiateItem(itemDropped, itemDroppedCollider);
        }
    }

    public override void UseItem(ObjectHolder _objectHolder)
    {

    }
}
