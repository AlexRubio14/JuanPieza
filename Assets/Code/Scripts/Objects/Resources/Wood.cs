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

    public override void Interact(ObjectHolder _objectHolder)
    {
        base.Interact(_objectHolder);
    }

    public override void UseItem(ObjectHolder _objectHolder)
    {
        Debug.Log("Uso madera");
    }
}
