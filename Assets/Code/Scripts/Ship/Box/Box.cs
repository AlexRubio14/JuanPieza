using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private GameObject itemDropped;

    public void DropItem(PlayerController player)
    {
        if(player.item == null)
        {
            GameObject item = Instantiate(itemDropped);
            player.SetItem(item);
        }
    }
}
