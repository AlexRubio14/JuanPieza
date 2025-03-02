using UnityEngine;

public abstract class Tool : InteractableObject
{
    [SerializeField] protected AudioClip dropItemClip;

    public bool addToolAtDestroy = true;
    public override void Interact(ObjectHolder _objectHolder)
    {

        if(!_objectHolder.GetHasObjectPicked())
        {
            PickItem(_objectHolder);
            return;
        }

        InteractableObject nearObj = _objectHolder.GetNearestInteractableObject();

        if (nearObj && nearObj is WoodShelf && nearObj.CanInteract(_objectHolder))
        {
            addToolAtDestroy = false;
            (nearObj as Box).AddItemInBox();
            InteractableObject currentObject = _objectHolder.RemoveItemFromHand();
            Destroy(currentObject.gameObject);
        }
        else
            DropItem(_objectHolder);

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
    public override void Use(ObjectHolder _objectHolder) { }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Box box = ShipsManager.instance.playerShip.GetObjectBoxByObject(objectSO);

        if (box != null && addToolAtDestroy)
            box.AddItemInBox();
        

    }

}
