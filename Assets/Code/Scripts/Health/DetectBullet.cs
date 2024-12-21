using UnityEngine;

public class DetectBullet : MonoBehaviour
{
    [Header("ShipInformation")]
    [SerializeField] private ShipHealth shipHealth;

    [Header("Hole")]
    [SerializeField] private GameObject hole;

    private Bullet bullet;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            bullet = collision.gameObject.GetComponent<Bullet>();
            shipHealth.SetCurrentHealth(-bullet.GetDamage());
            CreateHole(collision.contacts[0].point);
            Destroy(collision.gameObject);
        }
    }

    private void CreateHole(Vector3 position)
    {
        GameObject _hole = Instantiate(hole);
        _hole.transform.position = position;
        _hole.GetComponent<Hole>().SetShipInformation(bullet.GetDamage(), shipHealth);
        _hole.transform.SetParent(this.transform, true);
    }
}
