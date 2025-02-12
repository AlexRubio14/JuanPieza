using UnityEngine;

public class BulletsResource : Resource
{
    
    public override void Interact(ObjectHolder _objectHolder)
    {
        Debug.Log("Uso bala de canon");
        InteractableObject nearestObj = _objectHolder.GetNearestInteractableObject();
        if (nearestObj != null && nearestObj is Weapon && nearestObj.CanInteract(_objectHolder))
        {
            Weapon nearestWeapon = nearestObj as Weapon;

            if (!nearestWeapon.hasAmmo)
                nearestWeapon.Reload(_objectHolder);
        }
        else
        {
            base.Interact(_objectHolder);
        }

    }

    public override void Use(ObjectHolder _objectHolder) { }
}
