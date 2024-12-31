using UnityEngine;

public abstract class Tool : InteractableObject
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
        if (_objectHolder.GetHasObjectPicked())
        {
            DropItem(_objectHolder);
            return;
        }

        PickItem(_objectHolder);
    }

    private void PickItem(ObjectHolder _objectHolder)
    {
        GameObject item = _objectHolder.GetInteractableObject().gameObject;

        _objectHolder.GetInteractableObject().SetIsBeingUsed(true);

        item.transform.position = _objectHolder.GetObjectPickedPosition();
        item.transform.rotation = _objectHolder.transform.rotation;

        item.transform.SetParent(_objectHolder.transform.parent);

        _objectHolder.SetHasObjectPicked(true);

        selectedVisual.Hide();
    }

    private void DropItem(ObjectHolder _objectHolder)
    {
        GameObject item = _objectHolder.GetInteractableObject().gameObject;

        _objectHolder.GetInteractableObject().SetIsBeingUsed(false);

        item.transform.SetParent(null);

        _objectHolder.SetHasObjectPicked(false);

    }
}
