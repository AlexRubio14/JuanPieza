using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private List<ObjectSO> itemPool;

    [Space, Header("Priority List"), SerializeField]
    private List<ObjectSO> priorityList;
    
    [SerializedDictionary("Rarity", "Percentage")]
    public SerializedDictionary<ObjectSO.ItemRarity, float> rarityPercentages;
    [SerializeField]
    private List<float> realPercentages;

    private float totalPercentage;

    int totalBasics;
    int totalRares;
    int totalEpics;
    int totalLegendaries;

    [SerializeField] private int minCannonBallsInTheShip;

    private void Start()
    {
        totalPercentage = 0;
        foreach (KeyValuePair<ObjectSO.ItemRarity, float> item in rarityPercentages)
            totalPercentage += item.Value;

        StartCoroutine(WaitEndOfFrame());
        IEnumerator WaitEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            priorityList[0] = FindFirstObjectByType<Weapon>().objectSO;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            for (int i = 0; i < 100; i++)
            {
                GetRandomItem();
            }

            Debug.Log("Basicos " + totalBasics);
            Debug.Log("Raros " + totalRares);
            Debug.Log("Epicos " + totalEpics);
            Debug.Log("Legendarios " + totalLegendaries);
        }
    }

    public ObjectSO GetRandomItem()
    {
        ObjectSO priorityObject = GetObjectFromPriorityList();
        if (priorityObject != null)
            return priorityObject;


        ObjectSO.ItemRarity currentRarity = ObjectSO.ItemRarity.BASIC;

        float randNum = Random.Range(0, totalPercentage);
        float currentPercentage = 0;
        foreach (KeyValuePair<ObjectSO.ItemRarity, float> item in rarityPercentages)
        {
            if (randNum <= currentPercentage + item.Value)
            {
                currentRarity = item.Key;
                break;
            }

            currentPercentage += item.Value;
        }

        List<ObjectSO> currentRarityItems = new List<ObjectSO>();

        foreach (ObjectSO item in itemPool)
        {
            if(item.rarity == currentRarity)
                currentRarityItems.Add(item);
        }

        int randomItem = Random.Range(0, currentRarityItems.Count);

        switch (currentRarityItems[randomItem].rarity)
        {
            case ObjectSO.ItemRarity.BASIC:
                totalBasics++;
                break;
            case ObjectSO.ItemRarity.RARE:
                totalRares++;
                break;
            case ObjectSO.ItemRarity.EPIC:
                totalEpics++;
                break;
            case ObjectSO.ItemRarity.LEGENDARY:
                totalLegendaries++;
                break;
            default:
                break;
        }


        return currentRarityItems[randomItem];
    }

    private ObjectSO GetObjectFromPriorityList()
    {
        //Comprobar uno a uno los objetos 

        /* Cañones
         * Si no hay ninguna arma en el barco devolver un cañon
         */
        if (priorityList.Count >= 1 && !CheckObjectByType(priorityList[0]))
            return priorityList[0];

        /* Balas
         * Buscar la caja de balas y ver los objetos que tiene
         * Ver si hay balas suelta por el barco
         * Si no hay nada de balas ni en la caja ni en el barco devolver la bala
         */
        if (priorityList.Count >= 2 && GetNeedCannonBalls(priorityList[1]))
            return priorityList[1];

        return null;
    }

    private bool GetNeedCannonBalls(ObjectSO _object)
    {
        return (CheckObjectInsideBox(_object) + ShipsManager.instance.playerShip.GetTotalObjectQuantity(priorityList[1])) <= minCannonBallsInTheShip;
    }
    private int CheckObjectInsideBox(ObjectSO _object)
    {
        if (ShipsManager.instance.playerShip.GetObjectBoxByObject(_object) == null)
            return 0;

        return ShipsManager.instance.playerShip.GetObjectBoxByObject(_object).GetItemsInBox();
    }
    private bool CheckObjectByType(ObjectSO _object)
    {
        return ShipsManager.instance.playerShip.GetObjectOfType(_object.objectType).Count > 0;
    }

    public void AddItemToItemPool(ObjectSO _item)
    {
        itemPool.Add(_item);
    }
    public void RemoveItemFromPool(ObjectSO _item)
    {
        itemPool.Remove(_item);
    }
    public void AddItemToPriorityList(ObjectSO _item)
    {
        priorityList.Add(_item);
    }
    public void RemoveItemFromPriorityList(ObjectSO _item)
    {
        priorityList.Remove(_item);
    }    
    private void OnDrawGizmosSelected()
    {
        realPercentages = new List<float>();
        totalPercentage = 0;
        foreach (var item in rarityPercentages)
        {
            totalPercentage += item.Value;
        }

        foreach (var item in rarityPercentages)
        {
            realPercentages.Add((item.Value / totalPercentage) * 100);
        }
    }

    public void SetPercentages(SerializedDictionary<ObjectSO.ItemRarity, float> _rarityPercentages)
    {
        rarityPercentages = _rarityPercentages;
    }
}
