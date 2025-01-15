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

            if(other.TryGetComponent(out Catapult catapult))
            {
                ShipsManager.instance.playerShip.SetBarrelBox(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!MapManager.Instance.isVoting)
        {
            if (other.TryGetComponent(out InteractableObject interactableObject))
            {
                interactableObject.hasToBeInTheShip = false;
                ShipsManager.instance.playerShip.RemoveInteractuableObject(interactableObject);

                if (other.TryGetComponent(out Catapult catapult))
                {
                    ShipsManager.instance.playerShip.SetBarrelBox(false);
                }
            }
        }
    }
}
