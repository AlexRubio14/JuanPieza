using UnityEngine;

public class Repair : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private GameObject itemNeeded;
    public void RepairObject(PlayerController player)
    {
        if (player.item == itemNeeded)
        {
            //Activar particulas
            //Change State
        }
    }
}
