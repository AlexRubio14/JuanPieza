using UnityEngine;

public interface ICatapultAmmo
{
    public bool LoadItemInCatapult(ObjectHolder _objectHolder, InteractableObject _parentObject)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
        InteractableObject nearestObj = _objectHolder.GetNearestInteractableObject();
        if (handObject == _parentObject && nearestObj != null && nearestObj is Catapult && nearestObj.CanInteract(_objectHolder))
        {
            Catapult currentCatapult = nearestObj as Catapult;

            if (!currentCatapult.hasAmmo)
                currentCatapult.Reload(_objectHolder);
            _objectHolder.playerController.animator.SetBool("Pick", false);

            return true;
        }

        return false;
    }
}
