using UnityEngine;

public class Bullet : MonoBehaviour
{
    [field: SerializeField]
    public GameObject hitParticles { private set; get; }
    public AudioClip hitClip;
    public float explosionRadius;
    public bool createHole;

    protected float damage;
    protected bool damageDone;

    protected Transform shipImpacted;

    public void SetDamageDone(bool damageDone)
    { 
        this.damageDone = damageDone; 
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public bool GetDamageDone()
    {
        return damageDone;
    }

    public float GetDamage()
    {
        return damage;
    }

    public Transform GetShipImpacted()
    {
        return shipImpacted;
    }

    public void SetShipImpacted(Transform _shipImpacted)
    {
        shipImpacted = _shipImpacted;
    }
}
