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

    public override void Use(ObjectHolder _objectHolder)
    {
        
    }

    private void UpgradeShip()
    {
        AllyShip currentShip = ShipsManager.instance.playerShip;
        List<InteractableObject> currentInteractableObject = currentShip.GetInventory();

        Vector3 boatPosition = new Vector3(currentShip.transform.position.x, boatUpgrade.GetComponent<Ship>().GetInitY(), currentShip.transform.position.z);
        
        GameObject newBoat = Instantiate(boatUpgrade, boatPosition, Quaternion.identity);

        ShipsManager.instance.SetShip(newBoat.GetComponent<AllyShip>());
        
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
        Ship newShip = newBoat.GetComponent<Ship>();

        newShip.SetCurrentHealht(newShip.GetMaxHealth());
        newShip.SetDeafultTargetHeight();


        foreach (var players in PlayersManager.instance.ingamePlayers)
            players.gameObject.transform.SetParent(null, true);

        Destroy(currentShip.gameObject);

        ShipSceneManager.Instance.SetBoxesItem();
    }
}
