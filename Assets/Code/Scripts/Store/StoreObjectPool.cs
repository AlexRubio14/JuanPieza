using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreObjectPool : MonoBehaviour
{
    public enum ItemRarity { BASIC, RARE, EPIC, LEGENDARY }
    
    [Serializable]
    public struct Item
    {
        public string name;
        public GameObject prefab;
        public int price;
        public ItemRarity rarity;
    }
    
    [SerializeField] private Item[] itemPool;
    [SerializeField] private Item[] otherItemPool;
    [SerializeField] private Item[] weaponItemPool;

    [SerializedDictionary("Rarity", "Percentage")] public SerializedDictionary<ItemRarity, float> rarityPercentages;
    
    private float totalPercentage;
    
    public Item[] GetItemPool()
    {
        return itemPool;
    }

    public Item[] GetWeaponPool()
    {
        return weaponItemPool;
    }
    public Item[] GetOtherPool()
    {
        return otherItemPool;
    }

    public Item GetRandomItem(Item[] itemList)
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

        foreach (Item item in itemList)
        {
            if(item.rarity == currentRarity)
                currentRarityItems.Add(item);
        }

        int randomItem = UnityEngine.Random.Range(0, currentRarityItems.Count);
        
        return currentRarityItems[randomItem];
    }
}
