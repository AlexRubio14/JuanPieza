using UnityEngine;

public class ClusterBullet : Bullet
{
    [SerializeField] private GameObject explodedClusterCannonball;
    [SerializeField] private float clusterAmmo;

    private Rigidbody rb;
    private GameObject temp;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //lastVelocity = 0f
    }

    private void Update()
    {
        if (rb.linearVelocity.y < -12f)
        {
            Explode();
        }
    }

    private void Explode()
    {
        for (int i = 0; i < clusterAmmo; i++)
        {
            temp = Instantiate(explodedClusterCannonball, transform.position, Quaternion.identity);
            temp.GetComponent<Transform>().forward = transform.forward;
            temp.GetComponent<Bullet>().SetDamage(GetDamage());
        }

        Destroy(gameObject);
    }
}
