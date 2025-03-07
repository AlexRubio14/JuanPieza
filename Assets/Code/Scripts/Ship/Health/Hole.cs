using UnityEngine;

public class Hole : MonoBehaviour
{
    [Header("Particles")]
    [SerializeField] private ParticleSystem waterParticles;

    [Header("Time")]
    [SerializeField] private float maxDamageTime;
    [SerializeField] private int maxTimeLosingHealth;
    private float currentTime;
    private int currentTimeLosingHealth;

    private RepairHole hole;
    private float damageDiference;
    [SerializeField] private bool isPlayerHole;
    private void Start()
    {
        hole = GetComponent<RepairHole>();
        damageDiference = hole.GetDamageDeal() / maxTimeLosingHealth;
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
                if (isPlayerHole)
                {
                    hole.SetDamageDeal(damageDiference);
                    ShipsManager.instance.playerShip.SetRecoverHealth(damageDiference);
                }

                currentTime = 0;
                currentTimeLosingHealth++;
            }
        }
        else
        {
            waterParticles.Stop();
        }
    }
}
