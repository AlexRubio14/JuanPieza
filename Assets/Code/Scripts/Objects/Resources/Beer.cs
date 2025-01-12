using UnityEngine;

public class Beer : Resource
{
    public override void UseItem(ObjectHolder _objectHolder)
    {
        //playear animacion player

        _objectHolder.RemoveItemFromHand();
        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", false);
        Destroy(gameObject);
    }
}
