using System;
using UnityEngine;

public class StoreObjectSpawner : MonoBehaviour
{
    [SerializeField] private StoreObjectPool storeObjectPool;

    private StoreObjectPool.Item randomItem;
    private void Start()
    {
        InstanceRandomObject();
    }

    private void Update()
    {
        // Detecta que si no tiene hijos, gasta el dinero y borra el objeto padre
        if (transform.childCount == 0 && MoneyManager.Instance.SpendMoney(randomItem.price))
        {
            Destroy(gameObject);
        }
    }

    public void InstanceRandomObject()
    {
        randomItem = storeObjectPool.GetRandomItem(storeObjectPool.GetItemPool());

        if (randomItem.prefab != null)
        {
            Instantiate(randomItem.prefab, transform.position, transform.rotation, transform);
        }
    }
}
