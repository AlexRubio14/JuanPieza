using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    [Header("HealthVariables")]
    [SerializeField] private float maxHealth;
    private float currentHealth;

    [Header("LerpValues")]
    [SerializeField] private float heightChangeSpeed;
    private float initY;
    private float targetHeight; 
    private float currentHeight; 

    private void Start()
    {
        currentHealth = maxHealth;
        initY = transform.position.y;
        targetHeight = initY;
        currentHeight = transform.position.y; 
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
        FlotationLerp();
    }

    public void SetCurrentHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            //Destroy
        }
        if(currentHealth > maxHealth) 
        {
            currentHealth = maxHealth;
        }
        float healthFraction = currentHealth / maxHealth;
        targetHeight = Mathf.Lerp(0, initY, healthFraction);
    }

    private void FlotationLerp()
    {
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * heightChangeSpeed);
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
    }
}
