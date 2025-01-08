using UnityEngine;

public class Beer : Resource
{
    public override void UseItem(ObjectHolder _objectHolder)
    {
        //playear animacion player

        Destroy(this.gameObject);
    }
}
