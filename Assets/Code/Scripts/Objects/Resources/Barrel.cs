using UnityEngine;

public class Barrel : Resource
{
    public override void UseItem(ObjectHolder _objectHolder)
    {
        Debug.Log("Uso barril");
    }
}
