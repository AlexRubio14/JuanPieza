using System;
using UnityEngine;

public class StoreObjectSpawner : InteractableObject
{
    [SerializeField] private StoreObjectPool storeObjectPool;
    
    private InteractableObject interactableObject;
    private ObjectSO randomItem;
    private int lastChildCount;
    
    private void Start()
    {
        InstanceRandomObject();
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        OnItemPickedUp();
    }

    public override void UseItem(ObjectHolder _objectHolder)
    {
        throw new NotImplementedException();
    }

    private void OnItemPickedUp()
    {
        if (MoneyManager.Instance.SpendMoney(randomItem.price))
        {
            interactableObject.SetIsBeingUsed(false);
            Destroy(gameObject);
        }
    }

    public void InstanceRandomObject()
    {
        randomItem = storeObjectPool.GetRandomItem(storeObjectPool.GetItemPool());

        if (randomItem.prefab != null)
        {
            interactableObject = Instantiate(randomItem.prefab, transform.position, transform.rotation, transform).GetComponent<InteractableObject>();
            interactableObject.SetIsBeingUsed(true);
        }
    }
}
