using UnityEngine;

public class RepairHole : Repair
{
    private Ship ship;
    private float damageDeal;
    protected override void RepairEnded(ObjectHolder _objectHolder)
    {
        //ship.RemoveInteractuableObject(_objectHolder.GetHandInteractableObject());
        ship.SetCurrentHealth(damageDeal);
        InteractableObject currentObject = _objectHolder.RemoveItemFromHand();
        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", false);
        Destroy(currentObject.gameObject);
        Destroy(gameObject);
    }

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        if (handObject && handObject.objectSO == repairItem)
        {
            return new HintController.ActionType[] { HintController.ActionType.INTERACT, HintController.ActionType.HOLD_USE };
        }

        return new HintController.ActionType[] { HintController.ActionType.NONE };
    }
    public void SetbulletInformation(Ship _ship, float amount)
    {
        ship = _ship;
        damageDeal = amount;
    }
}
