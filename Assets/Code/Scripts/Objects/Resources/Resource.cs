using UnityEngine;

public abstract class Resource : InteractableObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        PickItem(_objectHolder);
    }

    private void PickItem(ObjectHolder _objectHolder)
    {
        GameObject item = _objectHolder.GetInteractableObject().gameObject;

        _objectHolder.GetInteractableObject().SetIsBeingUsed(true);

        item.transform.position = _objectHolder.GetObjectPickedPosition();
        item.transform.rotation = _objectHolder.transform.rotation;

        item.transform.SetParent(_objectHolder.transform.parent);
    }
    private void DropItem()
    {

    }
}
