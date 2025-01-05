using UnityEngine;

public class Wood : Resource
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UseItem(ObjectHolder _objectHolder)
    {
        //Repair objectRepair = _objectHolder.GetInteractableObject().GetComponent<Repair>();
        //if (objectRepair == null || objectRepair.GetItemNeeded().GetObjectName() != objectName)
        //    return;

        //objectRepair.Interact(_objectHolder);

        //Debug.Log("Uso madera");
    }
}
