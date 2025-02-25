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

    public override void Use(ObjectHolder _objectHolder)
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

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        };
    }
}
