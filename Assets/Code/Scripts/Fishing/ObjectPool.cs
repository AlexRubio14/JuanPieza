using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public enum ItemRarity { BASIC, RARE, EPIC, LEGENDARY }

    [Serializable]
    public struct Item
    {
        public string name;
        public GameObject prefab;
        public ItemRarity rarity;
    }

    [SerializeField] private Item[] itemPool;

    [SerializeField] private int[] priorityList;

    [SerializedDictionary("Rarity", "Percentage")]
    public SerializedDictionary<ItemRarity, float> rarityPercentages;

    private float totalPercentage;

    int totalBasics;
    int totalRares;
    int totalEpics;
    int totalLegendaries;

    private void Start()
    {
        totalPercentage = 0;
        foreach (KeyValuePair<ItemRarity, float> item in rarityPercentages)
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

    public Item GetRandomItem()
    {
        ItemRarity currentRarity = ItemRarity.BASIC;

        float randNum = UnityEngine.Random.Range(0, totalPercentage);
        float currentPercentage = 0;
        foreach (KeyValuePair<ItemRarity, float> item in rarityPercentages)
        {
            if (randNum <= currentPercentage + item.Value)
            {
                currentRarity = item.Key;
                break;
            }

            currentPercentage += item.Value;
        }

        List<Item> currentRarityItems = new List<Item>();

        foreach (Item item in itemPool)
        {
            if(item.rarity == currentRarity)
                currentRarityItems.Add(item);
        }

        int randomItem = UnityEngine.Random.Range(0, currentRarityItems.Count);

        switch (currentRarityItems[randomItem].rarity)
        {
            case ItemRarity.BASIC:
                totalBasics++;
                break;
            case ItemRarity.RARE:
                totalRares++;
                break;
            case ItemRarity.EPIC:
                totalEpics++;
                break;
            case ItemRarity.LEGENDARY:
                totalLegendaries++;
                break;
            default:
                break;
        }


        return currentRarityItems[randomItem];
    }


}
