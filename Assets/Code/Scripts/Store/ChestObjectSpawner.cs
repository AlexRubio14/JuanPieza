using UnityEngine;

public class ChestObjectSpawner : InteractableObject
{
    private enum TypeOfChest
    {
        Weapon,
        Totem,
        Item,
        Other
    }
    [SerializeField] private StoreObjectPool storeObjectPool;
    [SerializeField] private TypeOfChest chestType;
    
    [Header("Prices")]
    [SerializeField] private int chestWeaponPrice = 150;
    [SerializeField] private int chestOtherPrice = 100;
    public override void Interact(ObjectHolder _objectHolder)
    {
        switch (chestType)
        {
            case TypeOfChest.Weapon:
                InstanceRandomWeapon();
                break;
            case TypeOfChest.Totem:
                break;
            case TypeOfChest.Item:
                break;
            default:
                InstanceRandomOther();
                break;
        }
    }

    public override void UseItem(ObjectHolder _objectHolder)
    {
       
    }
    
    public void InstanceRandomWeapon()
    {
        if (MoneyManager.Instance.SpendMoney(chestWeaponPrice) || chestWeaponPrice == 0)
        {
            ObjectSO randomItem = storeObjectPool.GetRandomItem(storeObjectPool.GetWeaponPool());

            if (randomItem.prefab != null)
            {
                SpawnObject(randomItem);
            }
        }
    }
    
    public void InstanceRandomOther()
    {
        if (MoneyManager.Instance.SpendMoney(chestOtherPrice)|| chestOtherPrice == 0)
        {
            ObjectSO randomItem = storeObjectPool.GetRandomItem(storeObjectPool.GetOtherPool());

            if (randomItem.prefab != null)
            {
                SpawnObject(randomItem);
            }
        }
    }

    private void SpawnObject(ObjectSO _item)
    {
        InteractableObject interactableObject = Instantiate(_item.prefab, transform.position, transform.rotation).GetComponent<InteractableObject>();
        interactableObject.hasToBeInTheShip = false;
        Destroy(gameObject);
    }
}
