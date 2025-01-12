using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreObjectPool : MonoBehaviour
{
    [SerializeField] private ObjectSO[] itemPool;
    [SerializeField] private ObjectSO[] otherItemPool;
    [SerializeField] private ObjectSO[] weaponItemPool;

    [SerializedDictionary("Rarity", "Percentage")] public SerializedDictionary<ObjectSO.ItemRarity, float> rarityPercentages;
    
    private float totalPercentage;
    private void Awake()
    {
        totalPercentage = 0;
        foreach (KeyValuePair<ObjectSO.ItemRarity, float> item in rarityPercentages)
            totalPercentage += item.Value;
    }
    public ObjectSO[] GetItemPool()
    {
        return itemPool;
    }

    public ObjectSO[] GetWeaponPool()
    {
        return weaponItemPool;
    }
    public ObjectSO[] GetOtherPool()
    {
        return otherItemPool;
    }

    public ObjectSO GetRandomItem(ObjectSO[] itemList)
    {
        while (true)
        {
            ObjectSO.ItemRarity currentRarity = ObjectSO.ItemRarity.BASIC;
            float randNum = UnityEngine.Random.Range(0, totalPercentage);
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
            foreach (ObjectSO item in itemList)
            {
                if (item.rarity == currentRarity)
                {
                    currentRarityItems.Add(item);
                }
            }

            if (currentRarityItems.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, currentRarityItems.Count);
                return currentRarityItems[randomIndex];
            }
        }
    }
}
