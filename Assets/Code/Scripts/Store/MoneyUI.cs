using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Start()
    {
        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.OnMoneyChanged.AddListener(UpdateMoneyUI);
            UpdateMoneyUI(MoneyManager.Instance.GetMoney());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            MoneyManager.Instance.AddMoney(100);
        }
    }

    private void UpdateMoneyUI(int newMoney)
    {
        moneyText.text = $"Money: {newMoney}";
    }
}
