using UnityEngine;

public abstract class Tool : InteractableObject
{
    [SerializeField] protected AudioClip dropItemClip;

    protected bool addToolAtDestroy = true;

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
        InteractableObject nearObj = _objectHolder.GetNearestInteractableObject();

        if (nearObj && nearObj is WoodShelf && nearObj.CanInteract(_objectHolder))
        {
            addToolAtDestroy = false;
            (nearObj as Box).AddItemInBox();
            InteractableObject currentObject = _objectHolder.RemoveItemFromHand();
            Destroy(currentObject.gameObject);
        }
    }
    private void PickItem(ObjectHolder _objectHolder)
    {
        _objectHolder.ChangeObjectInHand(this);
        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", true);
        selectedVisual.Hide();
    }
    public void DropItem(ObjectHolder _objectHolder)
    {
        _objectHolder.RemoveItemFromHand();
        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", false);
        AudioManager.instance.Play2dOneShotSound(dropItemClip, "Objects");

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Box box = ShipsManager.instance.playerShip.GetObjectBoxByObject(objectSO);

        if (box != null && addToolAtDestroy)
            box.AddItemInBox();
        

    }

}
