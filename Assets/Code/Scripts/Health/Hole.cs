using UnityEngine;

public class Hole : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] private ParticleSystem waterParticles;

    [Header("Damage")]
    [SerializeField] private float holeDamage;
    private float damageDeal;

    [Header("Time")]
    [SerializeField] private float maxDamageTime;
    [SerializeField] private int maxTimeLosingHealth;
    private float currentTime;
    private int currentTimeLosingHealth;


    [Header("Ship")]
    private ShipHealth health;

    void Start()
    {
        currentTime = 0;
        currentTimeLosingHealth = 0;
    }


    void Update()
    {
        HoleDamageTimer();
    }

    private void HoleDamageTimer()
    {
        if (currentTimeLosingHealth < maxTimeLosingHealth)
        {
            currentTime += Time.deltaTime;
            if (currentTime > maxDamageTime)
            {
                health.SetCurrentHealth(-holeDamage);
                currentTime = 0;
                currentTimeLosingHealth++;
            }
        }
        else
        {
            waterParticles.Stop();
        }
    }


    public void InteractHole()
    {

    }

    public void SetShipInformation(float amount, ShipHealth _health)
    {
        damageDeal = amount;
        health = _health;
    }
}