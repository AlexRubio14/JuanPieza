using UnityEngine;

public class ShipWeigth : MonoBehaviour
{
    [Header("Weigth")]
    [SerializeField] private float maxWeigth;
    private float currentWeight;

    [Header("ShipHealth")]
    [SerializeField] private ShipHealth health;

    [Header("WeigthDamage")]
    [SerializeField] private float weigthDamage;

    [Header("Timer")]
    [SerializeField] private float damageTime;
    private float currentTime;
    void Start()
    {
        currentWeight = 0f;
        currentTime = 0;
    }

    void Update()
    {
        if (currentWeight >= maxWeigth)
        {
            //Enseñar mensaje
            currentTime += Time.deltaTime;
            if(currentTime > damageTime)
            {
                health.SetCurrentHealth(-weigthDamage);
                currentTime = 0;
            }
        }
        else
        {
            currentTime = 0;
        }
    }

    public void SetCurrentWeigth(float amount)
    {
        currentWeight += amount;
    }
}
