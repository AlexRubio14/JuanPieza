
public class BarrelResource : Resource, ICatapultAmmo
{
    public override void Release(ObjectHolder _objectHolder)
    {
        if (!(this as ICatapultAmmo).LoadItemInCatapult(_objectHolder, this))
            return;

        base.Release(_objectHolder);
    }
    public override void Interact(ObjectHolder _objectHolder) { }
    public override void Use(ObjectHolder _objectHolder) { }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return false;
    }
}
