using UnityEngine;

public class RockTutorial : DetectBullet
{
    [Space, Header("Tutorial Rock"), SerializeField]
    private GameObject breakParticlesPrefab;
    [SerializeField]
    private GameObject rockPiecePrefab;
    [SerializeField]
    private int rockPiecesCount;
    [SerializeField]
    private Vector2 rockXThrowForce;
    [SerializeField]
    private Vector2 rockYThrowForce;

    [SerializeField]
    private Vector2 rockTorqueForce;
    protected override void DetectCollision(Collision collision, Bullet _bullet)
    {
        if (_bullet.hitParticles)
            Instantiate(_bullet.hitParticles, collision.contacts[0].point, Quaternion.identity);
        if (_bullet.hitClip)
            AudioManager.instance.Play2dOneShotSound(_bullet.hitClip, "Objects");

        Destroy(collision.gameObject);

        //Romper la roca
        Instantiate(breakParticlesPrefab, collision.contacts[0].point, Quaternion.identity);

        for (int i = 0; i < rockPiecesCount; i++)
        {
            Rigidbody rb = Instantiate(rockPiecePrefab, collision.contacts[0].point, Quaternion.identity).GetComponent<Rigidbody>();
            Vector3 XFroce = new Vector3(Random.Range(rockXThrowForce.x, rockXThrowForce.y), 0, Random.Range(rockXThrowForce.x, rockXThrowForce.y));
            Vector3 YForce = new Vector3(0, Random.Range(rockYThrowForce.x, rockYThrowForce.y), 0);
            rb.AddForce(XFroce + YForce, ForceMode.Impulse);
            Vector3 torqueForce = new Vector3(
                Random.Range(rockTorqueForce.x, rockTorqueForce.y),
                Random.Range(rockTorqueForce.x, rockTorqueForce.y),
                Random.Range(rockTorqueForce.x, rockTorqueForce.y)
                );

            rb.AddTorque(torqueForce);
            Destroy(rb.gameObject, 10);
        }

        Destroy(gameObject);
    }
}
