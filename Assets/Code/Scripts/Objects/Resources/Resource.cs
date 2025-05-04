using UnityEngine;

public abstract class Resource : InteractableObject
{

    [SerializeField] protected AudioClip dropItemClip;

    public override void Grab(ObjectHolder _objectHolder)
    {
        if (!_objectHolder.GetHasObjectPicked())
            PickItem(_objectHolder);
    }
    public override void Release(ObjectHolder _objectHolder)
    {
        if (_objectHolder.GetHandInteractableObject() == this)
            DropItem(_objectHolder);
    }
    private void PickItem(ObjectHolder _objectHolder)
    {
        _objectHolder.ChangeObjectInHand(this);

        selectedVisual.Hide();

        //AudioManager.instance.Play2dOneShotSound(_objectHolder.pickUpClip, "Objects");

        _objectHolder.playerController.animator.SetBool("Pick", true);
    }
    private void DropItem(ObjectHolder _objectHolder)
    {
        _objectHolder.RemoveItemFromHand();
        AudioManager.instance.Play2dOneShotSound(dropItemClip, "Objects", 0.6f);
        _objectHolder.playerController.animator.SetBool("Pick", false);

    }

}
