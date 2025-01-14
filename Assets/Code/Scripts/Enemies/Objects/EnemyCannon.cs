using UnityEngine;
public class EnemyCannon : EnemyWeapon
{
    public override void UseObject()
    {
        base.UseObject();
        Rigidbody bulletRb = Instantiate(bullet, bulletSpawnPosition.position, Quaternion.identity).GetComponent<Rigidbody>();
        bulletRb.GetComponent<Bullet>().SetDamage(weaponDamage);
        bulletRb.AddForce(bulletSpawnPosition.forward * bulletForce, ForceMode.Impulse);
        Instantiate(shootParticles, bulletSpawnPosition.position, Quaternion.identity);
    }
}
