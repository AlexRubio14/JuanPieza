using UnityEngine;

public class ClusterBullet : Bullet
{
    [SerializeField] private GameObject explodedClusterCannonball;
    [SerializeField] private float clusterAmmo;
    [SerializeField] private float spawnOffset;

    private GameObject temp;

    private void OnDestroy()
    {
        Explode();
    }

    private void Explode()
    {
        for (int i = 0; i < clusterAmmo; i++)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, spawnOffset, 0);
            temp = Instantiate(explodedClusterCannonball, spawnPosition, Quaternion.identity);
            temp.GetComponent<Transform>().forward = transform.forward;
            temp.GetComponent<Bullet>().SetDamage(GetDamage()/clusterAmmo);
        }
    }
}
