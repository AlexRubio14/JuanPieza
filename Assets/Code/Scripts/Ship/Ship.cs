using UnityEngine;

public class Ship : MonoBehaviour
{
    [Header("HealthVariables")]
    [SerializeField] private float maxHealth;
    private float currentHealth;

    [Header("LerpValues")]
    [SerializeField] private float heightChangeSpeed;
    [SerializeField] private float lowerY;
    private float initY;
    private float targetHeight;
    private float currentHeight;

    [Header("Weigth")]
    [SerializeField] private float maxWeigth;
    private float currentWeight;

    [Header("WeigthDamage")]
    [SerializeField] private float weigthDamage;

    [Header("Timer")]
    [SerializeField] private float damageTime;
    private float currentTime;

    private void Start()
    {
        currentHealth = maxHealth;
        initY = transform.position.y;
        targetHeight = initY;
        currentHeight = transform.position.y;

        currentWeight = 0f;
        currentTime = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetCurrentHealth(-10f);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetCurrentHealth(10f);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetCurrentWeigth(100f);
        }
        FlotationLerp();
        WeightControl();
    }
    private void FlotationLerp()
    {
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * heightChangeSpeed);
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
    }

    private void WeightControl()
    {
        if (currentWeight >= maxWeigth)
        {
            //Enseñar mensaje
            currentTime += Time.deltaTime;
            if (currentTime > damageTime)
            {
                SetCurrentHealth(-weigthDamage);
                currentTime = 0;
            }
        }
        else
        {
            currentTime = 0;
        }
    }
    public void SetCurrentHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            //Destroy
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        float healthFraction = currentHealth / maxHealth;
        targetHeight = Mathf.Lerp(lowerY, initY, healthFraction);
    }
    public void SetCurrentWeigth(float amount)
    {
        currentWeight += amount;
    }
}
