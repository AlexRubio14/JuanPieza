using UnityEngine;

public class Hole : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] private ParticleSystem waterParticles;

    [Header("Damage")]
    [SerializeField] private float holeDamage;

    [Header("Time")]
    [SerializeField] private float maxDamageTime;
    [SerializeField] private int maxTimeLosingHealth;
    private float currentTime;
    private int currentTimeLosingHealth;

    [Header("Ship")]
    private Ship ship;

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
                ship.SetCurrentHealth(-holeDamage);
                currentTime = 0;
                currentTimeLosingHealth++;
            }
        }
        else
        {
            waterParticles.Stop();
        }
    }

    public void SetShipInformation(Ship _ship)
    {
        ship = _ship;
    }
}
