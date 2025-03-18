using UnityEngine;

public class DetectBullet : MonoBehaviour
{
    [Header("ShipInformation")]
    [SerializeField] protected Ship ship;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && collision.gameObject.TryGetComponent(out Bullet bullet) && !bullet.GetDamageDone())
        {
            if (ship)
            {
                bullet.SetShipImpacted(ship.transform);
                ShipsManager.instance.CheckLastEnemyShipHP();
            }
            AudioManager.instance.Play2dOneShotSound(bullet.hitClip, "Objects", 0.6f, 0.9f, 1.1f);
            DetectCollision(collision, bullet);
        }
    }

    protected virtual void DetectCollision(Collision collision, Bullet _bullet)
    {
        if (_bullet.hitParticles)
            Instantiate(_bullet.hitParticles, collision.contacts[0].point, Quaternion.identity);
        if(_bullet.hitClip)
            AudioManager.instance.Play2dOneShotSound(_bullet.hitClip, "Objects");

        _bullet.SetDamageDone(true);
        ship.SetCurrentHealth(-_bullet.GetDamage());
        if (ship.name == ShipsManager.instance.playerShip.name)
            Camera.main.GetComponent<CameraShaker>().TriggerShake(1);
        Destroy(collision.gameObject);
    }

}
