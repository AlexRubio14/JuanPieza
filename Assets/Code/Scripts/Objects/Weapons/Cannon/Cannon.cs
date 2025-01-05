using UnityEngine;

public class Cannon : Weapon
{
    [Space, Header("Cannon"), SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletSpawnPos;

    protected override void Shoot()
    {
        throw new System.NotImplementedException();
    }    
}
