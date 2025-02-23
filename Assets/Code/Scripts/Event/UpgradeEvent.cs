using System.Collections.Generic;
using UnityEngine;
using static ObjectSO;

public class UpgradeEvent : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private List<ObjectSO> weapons;

    [Header("Timer")]
    [SerializeField] private float maxTime;
    private float currentTime;

    private List<GameObject> currentsWeaponsInObject;

    private void Start()
    {
        currentsWeaponsInObject = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InteractableObject>() == null)
            return;

        if (other.GetComponent<InteractableObject>().objectSO.objectType == ObjectType.WEAPON)
        {
            currentTime = 0;
            currentsWeaponsInObject.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractableObject>() == null)
            return;

        if (other.GetComponent<InteractableObject>().objectSO.objectType == ObjectType.WEAPON)
        {
            currentsWeaponsInObject.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        if(currentsWeaponsInObject.Count == 1)
        {
            currentTime += Time.deltaTime;
            if (currentTime > maxTime)
                UpgradeCannon();
        }
    }

    private void UpgradeCannon()
    {
        ObjectSO currentWeaponSO = currentsWeaponsInObject[0].GetComponent<InteractableObject>().objectSO;

        ItemRarity upgradeRarity = currentWeaponSO.rarity;
        if (currentWeaponSO.rarity != ItemRarity.LEGENDARY)
            upgradeRarity = currentWeaponSO.rarity + 1;

        int randomWeapon = Random.Range(0, weapons.Count);

        while (weapons[randomWeapon].rarity != upgradeRarity || weapons[randomWeapon].name == currentWeaponSO.name)
            randomWeapon = Random.Range(0, weapons.Count);

        SpawnObject(randomWeapon);
    }

    private void SpawnObject(int index)
    {
        InteractableObject interactableObject = Instantiate(weapons[index].prefab, currentsWeaponsInObject[0].gameObject.transform.position, transform.rotation).GetComponent<InteractableObject>();
        interactableObject.hasToBeInTheShip = false;
        Destroy(currentsWeaponsInObject[0].gameObject);
        Destroy(gameObject);
    }
}
