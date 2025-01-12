using System;
using UnityEngine;

public class SetObjectIntoBoat : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Sail"))
            return;
        
        if (other.TryGetComponent(out InteractableObject interactableObject))
        {
            interactableObject.hasToBeInTheShip = true;
            ShipsManager.instance.playerShip.AddInteractuableObject(interactableObject, false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out InteractableObject interactableObject))
        {
            interactableObject.hasToBeInTheShip = false;
            ShipsManager.instance.playerShip.RemoveInteractuableObject(interactableObject);
        }
    }
}
