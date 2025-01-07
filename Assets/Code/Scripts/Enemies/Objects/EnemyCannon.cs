using UnityEngine;
public class EnemyCannon : EnemyWeapon
{
    public override void UseObject()
    {
        base.UseObject();
        Debug.Log("Dispara");
        Rigidbody bulletRb = Instantiate(bullet, bulletSpawnPosition.position, Quaternion.identity).GetComponent<Rigidbody>();
        bulletRb.AddForce(bulletSpawnPosition.forward * bulletForce, ForceMode.Impulse);
    }
}
