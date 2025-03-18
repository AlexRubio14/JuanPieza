using UnityEngine;

public class ClusterBullet : Bullet
{
    [SerializeField] private GameObject explodedClusterCannonball;
    [SerializeField] private float clusterAmmo;

    private GameObject temp;

    private void OnDestroy()
    {
        Explode();
    }

    private void Explode()
    {
        for (int i = 0; i < clusterAmmo; i++)
        {
            temp = Instantiate(explodedClusterCannonball, transform.position, Quaternion.identity);
            temp.GetComponent<Transform>().forward = transform.forward;
            temp.GetComponent<Bullet>().SetDamage(GetDamage()/clusterAmmo);
        }
    }
}
