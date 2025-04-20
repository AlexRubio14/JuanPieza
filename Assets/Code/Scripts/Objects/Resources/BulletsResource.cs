public class BulletsResource : Resource, ICatapultAmmo
{

    public override void Release(ObjectHolder _objectHolder)
    {
        if ((this as ICatapultAmmo).LoadItemInCatapult(_objectHolder, this))
            return;

        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
        InteractableObject nearestObj = _objectHolder.GetNearestInteractableObject();
        if (handObject == this && nearestObj != null && nearestObj is Weapon && nearestObj.CanInteract(_objectHolder))
        {
            Weapon nearestWeapon = nearestObj as Weapon;

            if (!nearestWeapon.hasAmmo)
                nearestWeapon.Reload(_objectHolder);
            _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", false);

            return;
        }

        base.Release(_objectHolder);
    }
    
    public override void Interact(ObjectHolder _objectHolder) { }
    public override void Use(ObjectHolder _objectHolder) { }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return false;
    }

}
