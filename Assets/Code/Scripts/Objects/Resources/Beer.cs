using UnityEngine;

public class Beer : Resource
{
    public override void Use(ObjectHolder _objectHolder)
    {
        //playear animacion player

        _objectHolder.RemoveItemFromHand();
        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", false);
        Destroy(gameObject);
    }
}
