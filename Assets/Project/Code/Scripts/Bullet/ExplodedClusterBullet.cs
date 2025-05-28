using UnityEngine;

public class ExplodedClusterBullet : Bullet
{
    [SerializeField] private Vector2 forceX;
    [SerializeField] private Vector2 forceY;
    [SerializeField] private Vector2 forceZ;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float randomAngleX = Random.Range(forceX.x, forceX.y);
        float randomAngleY = Random.Range(forceY.x, forceY.y);
        float randomAngleZ = Random.Range(forceZ.x, forceZ.y);
        Vector3 force = new Vector3(randomAngleX, randomAngleY, randomAngleZ);
        rb.AddForce(force, ForceMode.Impulse);
    }

    public Rigidbody GetRb()
    {
        return rb;
    }

}
