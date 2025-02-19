using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class BuffsManagers : MonoBehaviour
{
    public static BuffsManagers Instance { get; private set; }
    public List<Buffs> currentBuffs = new List<Buffs>();

    [Header("Damage Values")]
    [SerializeField] private float damageMultiplier;
    [SerializeField] private float explotePercentages;
    [SerializeField] private float explosionDamage;
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private AudioClip explosionAudio;
    private float currentDamageMultiplier;
    private float currentExplotionPercentages;

    [Header("Movement Values")]
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float knockBackMaxTime;

    [Header("Weight Values")]
    [SerializeField] private float weightMultiplier;
    [SerializeField] private GameObject cannon;
    private float currentWeightMultiplier;
    private bool canSteal;

    [Header("Gold Values")]
    [SerializeField] private float goldMultiplier;
    [SerializeField] private float loseGoldMaxTime;
    [SerializeField] private float loseGoldCuantity;
    private float currentGoldMultiplier;
    private bool loseGold;
    private float currentLoseGoldTime;

    [Header("Fishing Values")]
    [SerializedDictionary("Rarity", "Percentage")] public SerializedDictionary<ObjectSO.ItemRarity, float> boostRarityPercentages;
    private bool fishingEvent;

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
        InitValues();
    }

    private void InitValues()
    {
        currentDamageMultiplier = 1;
        currentExplotionPercentages = -1;

        currentWeightMultiplier = 1;
        canSteal = false;

        loseGold = false;
        currentGoldMultiplier = 1;
        currentLoseGoldTime = 0;

        fishingEvent = false;
    }

    public void InitBuffs()
    {
        if (MapManager.Instance.GetCurrentLevel().nodeType == NodeData.NodeType.BATTLE)
        {
            foreach (var buffs in currentBuffs)
            {
                switch (buffs.buffType)
                {
                    case Buffs.BuffsType.DAMAGE:
                        currentDamageMultiplier = damageMultiplier;
                        currentExplotionPercentages = explotePercentages;
                        break;
                    case Buffs.BuffsType.MOVEMENT:
                        foreach (var players in PlayersManager.instance.ingamePlayers)
                        {
                            players.SetBaseMovementSpeed(players.baseMovementSpeed * speedMultiplier);
                            players.movementBuffActive = true;
                        }
                        break;
                    case Buffs.BuffsType.STEAL:
                        currentWeightMultiplier = weightMultiplier;
                        canSteal = true;
                        break;
                    case Buffs.BuffsType.GOLD:
                        currentGoldMultiplier = goldMultiplier;
                        loseGold = true;
                        break;
                    case Buffs.BuffsType.FISHING:
                        FishingManager.instance.GetObjectPool().SetPercentages(boostRarityPercentages);
                        fishingEvent = true;
                        break;
                    case Buffs.BuffsType.REVIVE:
                        break;
                }
            }
        }
    }

    private void Update()
    {
        if(loseGold)
        {
            currentLoseGoldTime += Time.deltaTime;
            if(currentLoseGoldTime > loseGoldMaxTime)
            {
                currentLoseGoldTime = 0;
                MoneyManager.Instance.SpendMoney((int)loseGoldCuantity);
            }
        }
    }

    public void Steal()
    {
        GameObject _cannon = Instantiate(cannon);
        ShipsManager.instance.playerShip.AddInteractuableObject(_cannon.GetComponent<InteractableObject>(), true);
        GameObject ship = ShipsManager.instance.playerShip.gameObject;
        _cannon.transform.position = new Vector3(ship.transform.position.x, 10, ship.transform.position.z);
    }

    public void ResetBuffs()
    {
        for (int i = currentBuffs.Count - 1; i >= 0; i--)
        {
            currentBuffs[i].counter = currentBuffs[i].maxCounter;
            currentBuffs[i].counter--;
            if (currentBuffs[i].counter <= 0)
            {
                currentBuffs.RemoveAt(i);
            }
        }
    }

    public void ResetMovementSpeed()
    {
        foreach (var players in PlayersManager.instance.ingamePlayers)
        {
            players.movementBuffActive = false;
        }
    }

    public float GetCurrentDamageMultiplier()
    {
        return currentDamageMultiplier;
    }
    public float GetCurrentExplotionPercentages()
    {
        return currentExplotionPercentages;
    }
    public float GetExplosionDamage()
    {
        return explosionDamage;
    }
    public ParticleSystem GetExplosionParticles()
    {
        return explosionParticles;
    }
    public AudioClip GetExplosionAudio()
    {
        return explosionAudio;
    }
    public float GetKnockBackMaxTime()
    {
        return knockBackMaxTime;
    }
    public float GetWeightMultiplier()
    {
        return currentWeightMultiplier;
    }
    public bool GetCanSteal()
    {
        return canSteal;
    }
    public float GetCurrentGoldMultiplier()
    {
        return currentGoldMultiplier;
    }
    public bool GetLoseGold()
    {
        return loseGold;
    }
    public void SetLoseGold(bool state)
    {
        loseGold = state;
    }
    public bool GetIsFishingEvent()
    {
        return fishingEvent;
    }
}
