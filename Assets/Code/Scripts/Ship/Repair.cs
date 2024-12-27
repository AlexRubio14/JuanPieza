using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Repair : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private GameObject itemNeeded;

    [Header("Time")]
    [SerializeField] private float repairDuration;
    [SerializeField] private float repairSpeed;
    private float currentRepairTime;

    [Header("Player")]
    private List<PlayerController> players;

    [Header("Ship")]
    private Ship ship;
    private float damageDeal;

    private void Start()
    {
        players = new List<PlayerController>();
    }
    public void StartRepairObject(PlayerController player)
    {
        if (player.item == itemNeeded)
        {
            players.Add(player);
            //Change State
        }
    }

    public void StopRepairObject(PlayerController player)
    {
        players.Remove(player);
        //Change State
    }

    private void Update()
    {
        if(players.Count > 0)
        {
            currentRepairTime += repairSpeed * players.Count * Time.deltaTime;
            if (currentRepairTime >= repairDuration)
            {
                FinishRepairing();
                ship.SetCurrentHealth(damageDeal);
                currentRepairTime = 0;
            }
        }
        else
        {
            currentRepairTime = 0;
        }
    }

    private void FinishRepairing()
    {
        for(int i = players.Count - 1; i >= 0; i--) 
        {
            RepairEnded(players[i]);
            StopRepairObject(players[i]);
        }

    }

    protected virtual void RepairEnded(PlayerController player)
    {

    }

    public void SetbulletInformation(Ship _ship, float amount)
    {
        ship = _ship;
        damageDeal = amount;
    }
}
