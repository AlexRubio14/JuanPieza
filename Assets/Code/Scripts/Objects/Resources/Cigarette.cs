using UnityEngine;
public class Cigarette : Resource, ICatapultAmmo
{
    [SerializeField]
    private AudioClip smokeClip;
    public override void Release(ObjectHolder _objectHolder)
    {
        if ((this as ICatapultAmmo).LoadItemInCatapult(_objectHolder, this))
            return;

        base.Release(_objectHolder);
    }
    public override void Interact(ObjectHolder _objectHolder) { }
    public override void Use(ObjectHolder _objectHolder) 
    {
        if (ShipsManager.instance != null)
            ShipsManager.instance.playerShip.Smoke();

        _objectHolder.playerController.animator.SetTrigger("Consume"); //Animacion de empezar a fumar
        _objectHolder.GetComponentInParent<CigaretteController>().ActivateCigarrette();
        _objectHolder.RemoveItemFromHand();

        _objectHolder.playerController.animator.SetBool("Pick", false);
        AudioManager.instance.Play2dOneShotSound(smokeClip, "Objects", 0.8f);
        Destroy(gameObject);
    }

    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return _objectHolder.GetHasObjectPicked() == this;
    }

}
