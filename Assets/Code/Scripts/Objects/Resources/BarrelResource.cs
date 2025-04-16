using UnityEngine;

public class BarrelResource : Resource, ICatapultAmmo
{
    public override void Interact(ObjectHolder _objectHolder)
    {
        if (!(this as ICatapultAmmo).LoadItemInCatapult(_objectHolder, this))
            base.Interact(_objectHolder);
    }
    public override void Use(ObjectHolder _objectHolder)
    {

    }
}
