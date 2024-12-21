using UnityEngine;

public class DetectBullet : MonoBehaviour
{
    [Header("ShipInformation")]
    [SerializeField] private Ship shipHealth;

    [Header("Hole")]
    [SerializeField] private GameObject hole;

    private Bullet bullet;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !collision.gameObject.GetComponent<Bullet>().GetDamageDone())
        {
            bullet = collision.gameObject.GetComponent<Bullet>();
            bullet.SetDamageDone(true);
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
