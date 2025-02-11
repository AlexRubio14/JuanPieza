using UnityEngine;

public class SellObjects : InteractableObject
{
    public override void Interact(ObjectHolder _objectHolder)
    {
        if (_objectHolder.GetHasObjectPicked())
        {
            MoneyManager.Instance.AddMoney(_objectHolder.GetHandInteractableObject().objectSO.price);
            Destroy(_objectHolder.GetHandInteractableObject().gameObject);
            _objectHolder.RemoveItemFromHand();
        }
    }

    public override void Use(ObjectHolder _objectHolder)
    {
    }
}
