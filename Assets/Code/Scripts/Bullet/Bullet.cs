using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damage;

    private bool damageDone;

    public void SetDamageDone(bool damageDone)
    { 
        this.damageDone = damageDone; 
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
