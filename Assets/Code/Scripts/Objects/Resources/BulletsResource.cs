using UnityEngine;

public class BulletsResource : Resource
{

    public override void Use(ObjectHolder _objectHolder) 
    {
        InteractableObject nearestObj = _objectHolder.GetNearestInteractableObject();
        if (nearestObj != null && nearestObj is Weapon && nearestObj.CanInteract(_objectHolder))
        {
            Weapon nearestWeapon = nearestObj as Weapon;

            if (!nearestWeapon.hasAmmo)
                nearestWeapon.Reload(_objectHolder);
            _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", false);
        }
    }
}
