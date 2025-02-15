using UnityEngine;

public class EventChest : InteractableObject
{
    private enum TypeOfChest
    {
        Weapon,
        Totem,
        Item,
        Other
    }
    [SerializeField] private StoreObjectPool storeObjectPool;
    [SerializeField] private ChestController chestController;
    private TypeOfChest chestType;

    private bool canInteract;

    protected override void Start()
    {
        base.Start();
        canInteract = true;
        chestType = (TypeOfChest)Random.Range(0, System.Enum.GetValues(typeof(TypeOfChest)).Length);
    }
    public override void Interact(ObjectHolder _objectHolder)
    {
        if (!canInteract)
            return;

        switch (chestType)
        {
            case TypeOfChest.Weapon:
                InstanceRandomWeapon();
                break;
            case TypeOfChest.Totem:
                break;
            case TypeOfChest.Item:
                InstanceRandomItem();
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
        ObjectSO randomItem = storeObjectPool.GetRandomItem(storeObjectPool.GetWeaponPool());

        if (randomItem.prefab != null)
        {
            SpawnObject(randomItem);
        }
    }

    public void InstanceRandomOther()
    {
        ObjectSO randomItem = storeObjectPool.GetRandomItem(storeObjectPool.GetOtherPool());

        if (randomItem.prefab != null)
        {
            SpawnObject(randomItem);
        }
    }

    public void InstanceRandomItem()
    {
        ObjectSO randomItem = storeObjectPool.GetRandomItem(storeObjectPool.GetItemPool());

        if (randomItem.prefab != null)
        {
            SpawnObject(randomItem);
        }
    }

    private void SpawnObject(ObjectSO _item)
    {
        InteractableObject interactableObject = Instantiate(_item.prefab, transform.position, transform.rotation).GetComponent<InteractableObject>();
        interactableObject.hasToBeInTheShip = false;
        chestController.DesactiveChests();
        Destroy(gameObject);
    }

    public void SetCanInteract(bool _canInteract)
    {
        canInteract = _canInteract;
    }
}
