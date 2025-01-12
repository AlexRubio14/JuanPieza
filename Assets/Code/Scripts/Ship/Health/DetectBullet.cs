using UnityEngine;

public class DetectBullet : MonoBehaviour
{
    [Header("ShipInformation")]
    [SerializeField] protected Ship ship;

    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && collision.gameObject.TryGetComponent(out Bullet bullet) && !bullet.GetDamageDone())
        {
            DetectCollision(collision, bullet);
        }
    }

    protected virtual void DetectCollision(Collision collision, Bullet _bullet)
    {
        _bullet.SetDamageDone(true);
        ship.SetCurrentHealth(-_bullet.GetDamage());
        Destroy(collision.gameObject);
    }

}
