using UnityEngine;

public class BulletsResource : Resource
{

    public override void Interact(ObjectHolder _objectHolder)
    {

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

        base.Interact(_objectHolder);
    }
    public override void Use(ObjectHolder _objectHolder) 
    {
        
    }
}
