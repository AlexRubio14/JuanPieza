using UnityEngine;

public class Cannon : Weapon
{
    [Space, Header("Cannon"), SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletSpawnPos;

    protected override void Start()
    {
        base.Start();
        AddLoadParticle(bulletSpawnPos);
    }
    protected override void Shoot()
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPos.transform.position, Quaternion.identity);
        newBullet.GetComponent<Bullet>().SetDamage(weaponDamage);
        newBullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPos.forward * bulletForce, ForceMode.Impulse);
        Instantiate(shootParticles, bulletSpawnPos.position, Quaternion.identity);
        AudioManager.instance.Play2dOneShotSound(weaponShootClip, "Objects");
    }

}
