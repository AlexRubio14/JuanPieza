using UnityEditor;
using UnityEngine;

public class Sword : Tool
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

    protected override void UseItem()
    {
        Debug.Log("Corte con la espada");
    }
}
