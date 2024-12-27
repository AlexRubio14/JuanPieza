using Unity.VisualScripting;
using UnityEngine;

public class DetectBullet : MonoBehaviour
{
    [Header("ShipInformation")]
    [SerializeField] protected Ship ship;

    protected Bullet bullet { get; private set; }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !collision.gameObject.GetComponent<Bullet>().GetDamageDone())
        {
            DetectCollision(collision);
        }
    }

    protected virtual void DetectCollision(Collision collision)
    {
        bullet = collision.gameObject.GetComponent<Bullet>();
        bullet.SetDamageDone(true);
        ship.SetCurrentHealth(-bullet.GetDamage());
        Destroy(collision.gameObject);
    }

}
