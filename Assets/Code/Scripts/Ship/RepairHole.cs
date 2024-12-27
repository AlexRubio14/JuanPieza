using UnityEngine;

public class RepairHole : Repair
{
    protected override void RepairEnded(PlayerController player)
    {
        ship.RemoveInteractuableObject(player.item);
        player.SetItem(null);
        //Destruir el item
        Destroy(gameObject);
    }
}
