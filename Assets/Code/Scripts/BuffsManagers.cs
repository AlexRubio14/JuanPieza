using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buffs
{
    public enum BuffsType { DAMAGE, MOVEMENT, STEAL, GOLD, FISHING}

    public BuffsType buffType;
    public int counter;
}

public class BuffsManagers : MonoBehaviour
{
    public static BuffsManagers Instance { get; private set; }
    private List<Buffs> currentBuffs = new List<Buffs>();

    [Header("Damage Values")]
    [SerializeField] private float damageMultiplier;
    [SerializeField] private float explotePercentages;
    private float currentDamageMultiplier;
    private float currentExplotePercentages;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        currentDamageMultiplier = 2;
        currentExplotePercentages = -1;
    }

    private void Start()
    {
        foreach (var buffs in currentBuffs) 
        { 
            switch(buffs.buffType)
            {
                case Buffs.BuffsType.DAMAGE:
                    currentDamageMultiplier = damageMultiplier;
                    currentExplotePercentages = explotePercentages;
                    break;
                case Buffs.BuffsType.MOVEMENT:
                    break;
                case Buffs.BuffsType.STEAL:
                    break;
                case Buffs.BuffsType.GOLD:
                    break;
                case Buffs.BuffsType.FISHING:
                    break;
            }
        }
    }

    public float GetCurrentDamageMultiplier()
    {
        return currentDamageMultiplier;
    }

    public float GetCurrentExplotePercentages()
    {
        return currentExplotePercentages;
    }
}
