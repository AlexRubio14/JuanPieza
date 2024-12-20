using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damage;

    public float GetDamage()
    {
        return damage;
    }
}
