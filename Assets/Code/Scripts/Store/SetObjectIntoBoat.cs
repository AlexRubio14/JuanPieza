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

        if (other.CompareTag("Player") && ShipsManager.instance.playerShip.gameObject.transform.Find("Sail").GetComponentInChildren<ShippingSail>().GetIsBridgeActive())
            other.gameObject.transform.SetParent(ShipsManager.instance.playerShip.gameObject.transform, true);

    }

    private void OnTriggerExit(Collider other)
    {
        if (!MapManager.Instance.isVoting)
        {
            if (other.TryGetComponent(out InteractableObject interactableObject))
            {
                interactableObject.hasToBeInTheShip = false;
                ShipsManager.instance.playerShip.RemoveInteractuableObject(interactableObject);
            }
        }

        if (other.CompareTag("Player") && ShipsManager.instance.playerShip.gameObject.transform.Find("Sail").GetComponentInChildren<ShippingSail>().GetIsBridgeActive())
            other.gameObject.transform.SetParent(null, true);
    }
}
