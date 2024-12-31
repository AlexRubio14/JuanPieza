using UnityEngine;

public class Cannonball : Resource
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
        Debug.Log("Uso bala de cañon");
    }
}
