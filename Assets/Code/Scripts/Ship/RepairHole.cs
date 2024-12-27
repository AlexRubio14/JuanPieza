using UnityEngine;

public class RepairHole : Repair
{
    protected override void RepairEnded(PlayerController player)
    {
        player.SetItem(null);
        //Destruir el item
        Destroy(gameObject);
    }
}
