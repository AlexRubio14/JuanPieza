using UnityEngine;

public class Bullet : MonoBehaviour
{
    [field: SerializeField]
    public GameObject hitParticles;

    private float damage;

    private bool damageDone;

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
}
