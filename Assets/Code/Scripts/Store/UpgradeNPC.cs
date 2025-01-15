using System.Collections.Generic;
using UnityEngine;

public class UpgradeNPC : InteractableObject
{
    [SerializeField] private int upgradeCost = 500;
    [SerializeField] private GameObject boatUpgrade;
    public override void Interact(ObjectHolder _objectHolder)
    {
        if (MoneyManager.Instance.SpendMoney(upgradeCost))
            UpgradeShip();
    }

    public override void UseItem(ObjectHolder _objectHolder)
    {
        
    }

    private void UpgradeShip()
    {
        Ship currentShip = ShipsManager.instance.playerShip;
        List<InteractableObject> currentInteractableObject = currentShip.GetInventory();
        
        GameObject newBoat = Instantiate(boatUpgrade, currentShip.transform.position, Quaternion.identity);

        ShipsManager.instance.SetShip(newBoat.GetComponent<Ship>());
        
        foreach (InteractableObject interactableObject in currentInteractableObject)
        {
            if (interactableObject.objectSO.objectType != ObjectSO.ObjectType.BOX)
            {
                interactableObject.transform.SetParent(newBoat.transform);
                interactableObject.transform.position = new Vector3(interactableObject.transform.position.x, 10, interactableObject.transform.position.z);
                ShipsManager.instance.playerShip.AddInteractuableObject(interactableObject);
                if (interactableObject.hasToBeInTheShip)
                {
                    interactableObject.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                }
            }
            else
            {
                ShipSceneManager.Instance.SaveBoxData(interactableObject.objectSO, interactableObject as Box);
            }
        }
        
        
        
        Destroy(currentShip.gameObject);

        ShipSceneManager.Instance.SetBoxesItem();
    }
}
