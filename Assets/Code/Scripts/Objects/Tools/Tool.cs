using UnityEngine;

public abstract class Tool : InteractableObject
{
    [SerializeField] protected AudioClip dropItemClip;

    public bool addToolAtDestroy { set; get; } = true;



    public override void Grab(ObjectHolder _objectHolder)
    {
        if (!_objectHolder.GetHasObjectPicked())
            PickItem(_objectHolder);
    }
    public override void Release(ObjectHolder _objectHolder)
    {
        DropItem(_objectHolder);
    }
    public override void Interact(ObjectHolder _objectHolder)
    {
        
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return false;
    }

    private void PickItem(ObjectHolder _objectHolder)
    {
        _objectHolder.ChangeObjectInHand(this);
        _objectHolder.playerController.animator.SetBool("Pick", true);
        selectedVisual.Hide();
    }
    public void DropItem(ObjectHolder _objectHolder)
    {
        _objectHolder.RemoveItemFromHand();
        _objectHolder.playerController.animator.SetBool("Pick", false);
        AudioManager.instance.Play2dOneShotSound(dropItemClip, "Objects");

    }

    protected override void OnDestroy()
    {
        Box box = ShipsManager.instance.playerShip.GetObjectBoxByObject(objectSO);

        if (box != null && addToolAtDestroy)
            box.AddItemInBox();
        

        base.OnDestroy();
    }

}
