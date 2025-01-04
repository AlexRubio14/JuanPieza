using UnityEngine;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour
{
 public static MoneyManager Instance { get; private set; }

    [SerializeField] private int startingMoney = 100;
    private int currentMoney;

    public UnityEvent<int> OnMoneyChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentMoney = startingMoney;
        OnMoneyChanged?.Invoke(currentMoney);
    }

    public int GetMoney()
    {
        return currentMoney;
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("Attempted to spend a non-positive amount of money.");
            return false;
        }

        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            OnMoneyChanged?.Invoke(currentMoney);
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough money to spend.");
            return false;
        }
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("Attempted to add a non-positive amount of money.");
            return;
        }

        currentMoney += amount;
        OnMoneyChanged?.Invoke(currentMoney);
    }
}
