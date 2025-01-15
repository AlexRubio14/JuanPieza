using UnityEngine;

public class Tool : InteractableObject
{
    [SerializeField] protected AudioClip dropItemClip;

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
        _objectHolder.ChangeObjectInHand(this);

        selectedVisual.Hide();
    }

    public void DropItem(ObjectHolder _objectHolder)
    {
        _objectHolder.RemoveItemFromHand();
        AudioManager.instance.Play2dOneShotSound(dropItemClip, "Objects");
    }
    public override void UseItem(ObjectHolder _objectHolder) { }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Box box = ShipsManager.instance.playerShip.GetObjectBoxByObject(objectSO);

        if (box != null)
            box.AddItemInBox();
        

    }
}
