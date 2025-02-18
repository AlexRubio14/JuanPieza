using UnityEngine;

public class CannonDouble : Weapon
{
    [Space, Header("CannonDouble"), SerializeField]
    private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPosRight;
    [SerializeField] private Transform bulletSpawnPosLeft;

    protected override void Start()
    {
        base.Start();
        AddLoadParticle(bulletSpawnPosRight);
        AddLoadParticle(bulletSpawnPosLeft);
    }
    protected override void Shoot()
    {
        GameObject newBulletRight = Instantiate(bulletPrefab, bulletSpawnPosRight.transform.position, Quaternion.identity);
        newBulletRight.GetComponent<Rigidbody>().AddForce(bulletSpawnPosRight.forward * bulletForce, ForceMode.Impulse);
        newBulletRight.GetComponent<Bullet>().SetDamage(weaponDamage * BuffsManagers.Instance.GetCurrentDamageMultiplier());
        GameObject newBulletLeft = Instantiate(bulletPrefab, bulletSpawnPosLeft.transform.position, Quaternion.identity);
        newBulletLeft.GetComponent<Rigidbody>().AddForce(bulletSpawnPosLeft.forward * bulletForce, ForceMode.Impulse);
        newBulletLeft.GetComponent<Bullet>().SetDamage(weaponDamage * BuffsManagers.Instance.GetCurrentDamageMultiplier());

        hasAmmo = false;
        Instantiate(shootParticles, bulletSpawnPosRight.position, Quaternion.identity);
        Instantiate(shootParticles, bulletSpawnPosLeft.position, Quaternion.identity);
    }
}
