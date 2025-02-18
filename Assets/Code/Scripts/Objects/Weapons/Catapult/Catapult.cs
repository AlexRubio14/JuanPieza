using UnityEngine;

public class Catapult : Weapon
{
    [Space, Header("Catapult"), SerializeField]
    private GameObject barrelPrefab;
    [SerializeField]
    private Transform barrelSpawnPos;
    [SerializeField]
    private float barrelRotationSpeed;
    protected override void Shoot()
    {
        GameObject newBullet = Instantiate(barrelPrefab, barrelSpawnPos.transform.position, Quaternion.identity);
        Rigidbody bulletRB =  newBullet.GetComponent<Rigidbody>();
        bulletRB.AddForce(barrelSpawnPos.forward * bulletForce, ForceMode.Impulse);
        bulletRB.AddRelativeTorque(
            new Vector3(
                Random.Range(-barrelRotationSpeed, barrelRotationSpeed), 
                Random.Range(-barrelRotationSpeed, barrelRotationSpeed), 
                Random.Range(-barrelRotationSpeed, barrelRotationSpeed)
                )
            , ForceMode.VelocityChange);
        newBullet.GetComponent<Bullet>().SetDamage(weaponDamage * BuffsManagers.Instance.GetCurrentDamageMultiplier());
        hasAmmo = false;
    }
}
