using System;
using UnityEngine;

public class StoreObjectSpawner : InteractableObject
{
    [SerializeField] private StoreObjectPool storeObjectPool;
    
    private InteractableObject interactableObject;
    private ObjectSO randomItem;
    private int lastChildCount;
    
    protected override void Start()
    {
        base.Start();
        InstanceRandomObject();
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        OnItemPickedUp();
    }

    public override void Use(ObjectHolder _objectHolder)
    {
        
    }

    private void OnItemPickedUp()
    {
        if (MoneyManager.Instance.SpendMoney(randomItem.price))
        {
            EnableCollisions(interactableObject, true);
            UnparentAllChildren(gameObject);
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

    private void UnparentAllChildren(GameObject parentObject)
    {
        for (int i = parentObject.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = parentObject.transform.GetChild(i);
            
            child.SetParent(null);
        }
    }
}
