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
        if(isBeingUsed)
        {
            return;
        }

        if(_objectHolder.GetObjectPicked() == null) 
        {
            _objectHolder.PickObject();
        }
        else
        {
            UseItem();
        }
        
    }

    protected abstract void UseItem();
}
