using UnityEngine;
public class EnemyCannon : EnemyWeapon
{  
    public override void UseObject()
    {
        base.UseObject();
        Debug.Log("Dispara");
    }
}
