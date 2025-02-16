using UnityEngine;

public class ExplodedClusterBullet : Bullet
{
    [SerializeField] private float maxAngle;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float randomAngleX = Random.Range(-maxAngle, maxAngle);
        float randomAngleY = Random.Range(0, maxAngle);
        float randomAngleZ = Random.Range(-maxAngle, maxAngle);
        Vector3 direction = new Vector3(randomAngleX, randomAngleY, randomAngleZ).normalized;
        rb.AddForce(direction * 150, ForceMode.Impulse);
    }

    public Rigidbody GetRb()
    {
        return rb;
    }

}
