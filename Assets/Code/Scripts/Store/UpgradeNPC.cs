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
            interactableObject.transform.SetParent(newBoat.transform);
            interactableObject.transform.position = new Vector3(interactableObject.transform.position.x, 30,
                interactableObject.transform.position.z);
        }
        
        Destroy(currentShip.gameObject);

    }
}
