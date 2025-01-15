using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private ObjectSO[] itemPool;

    [Space, Header("Priority List"), SerializeField]
    private ObjectSO[] priorityList;
    
    [SerializedDictionary("Rarity", "Percentage")]
    public SerializedDictionary<ObjectSO.ItemRarity, float> rarityPercentages;
    [SerializeField]
    private List<float> realPercentages;

    private float totalPercentage;

    int totalBasics;
    int totalRares;
    int totalEpics;
    int totalLegendaries;

    private void Start()
    {
        totalPercentage = 0;
        foreach (KeyValuePair<ObjectSO.ItemRarity, float> item in rarityPercentages)
            totalPercentage += item.Value;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
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

        /*Comprobar madera
         * Buscar la caja de madera y ver los objetos que tiene
         * Ver si hay madera suelta por el barco
         * Si no hay nada de madera en la caja ni en el barco devolver la madera
         */
        if (!CheckObjectInsideBox(priorityList[0]) && !ShipsManager.instance.playerShip.ItemExist(priorityList[0]))
            return priorityList[0];
        /* Cañones
         * Si no hay ninguna arma en el barco devolver un cañon
         */
        if (!CheckObjectByType(priorityList[1]))
            return priorityList[1];

        /* Balas
         * Buscar la caja de balas y ver los objetos que tiene
         * Ver si hay balas suelta por el barco
         * Si no hay nada de balas ni en la caja ni en el barco devolver la bala
         */
        if (!CheckObjectInsideBox(priorityList[2]) && !ShipsManager.instance.playerShip.ItemExist(priorityList[2]))
            return priorityList[2];

        return null;
    }
    private bool CheckObjectInsideBox(ObjectSO _object)
    {
        return ShipsManager.instance.playerShip.GetObjectBoxByObject(_object).HasItems();
    }
    private bool CheckObjectByType(ObjectSO _object)
    {
        return ShipsManager.instance.playerShip.GetObjectOfType(_object.objectType).Count > 0;
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
}
