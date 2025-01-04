using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreObjectPool : MonoBehaviour
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
    [SerializedDictionary("Rarity", "Percentage")] public SerializedDictionary<ItemRarity, float> rarityPercentages;
    
    private float totalPercentage;
    
    private void Start()
    {
        Item randomItem = GetRandomItem();

        if (randomItem.prefab != null)
        {
            Instantiate(randomItem.prefab, transform.position, transform.rotation);
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
        
        return currentRarityItems[randomItem];
    }
}
