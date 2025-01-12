using UnityEngine;

public class Catapult : Weapon
{
    [Space, Header("Catapult"), SerializeField]
    private GameObject barrelPrefab;
    [SerializeField]
    private Transform barrelSpawnPos;

    protected override void Shoot()
    {
        GameObject newBullet = Instantiate(barrelPrefab, barrelSpawnPos.transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().AddForce(barrelSpawnPos.forward * bulletForce, ForceMode.Impulse);
        newBullet.GetComponent<Bullet>().SetDamage(weaponDamage);
        hasAmmo = false;
    }
}
