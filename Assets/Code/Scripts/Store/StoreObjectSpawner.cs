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
        
    }

    private void OnItemPickedUp()
    {
        if (MoneyManager.Instance.SpendMoney(randomItem.price))
        {
            EnableCollisions(interactableObject, true);
            Destroy(gameObject);
        }
    }

    public void InstanceRandomObject()
    {
        randomItem = storeObjectPool.GetRandomItem(storeObjectPool.GetItemPool());

        if (randomItem.prefab != null)
        {
            interactableObject = Instantiate(randomItem.prefab, transform.position, transform.rotation, transform).GetComponent<InteractableObject>();
            EnableCollisions(interactableObject, false);
            interactableObject.hasToBeInTheShip = false;
        }
    }

    private void EnableCollisions(InteractableObject interactableObject, bool enable)
    {
        if (interactableObject.TryGetComponent(out Collider interactableObjectCollider))
            interactableObjectCollider.enabled = enable;

        if (interactableObject.TryGetComponent(out Rigidbody interactableObjectRigidbody))
            interactableObjectRigidbody.isKinematic = !enable;
    }
}
