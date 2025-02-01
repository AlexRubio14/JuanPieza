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

    public void SetbulletInformation(Ship _ship, float amount)
    {
        ship = _ship;
        damageDeal = amount;
    }
}
