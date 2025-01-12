using UnityEngine;

public class Cigarrette : Resource
{
    public override void UseItem(ObjectHolder _objectHolder)
    {
        //playear animacion player
        _objectHolder.GetComponentInParent<CigarretteController>().ActivateCigarrette();
        
    }
}
