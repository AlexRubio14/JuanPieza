using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private InteractableObject itemDropped;
    private int itemsInBox;

    public void DropItem(PlayerController player)
    {
        if(player.item == null && HasItem())
        {
            Instantiate(itemDropped.gameObject);
            player.SetItem(itemDropped);
            RemoveItemInBox();
        }
    }

    public void AddItemInBox()
    {
        itemsInBox++;
    }

    public void RemoveItemInBox()
    {
        itemsInBox--;
    }

    public bool HasItem()
    {
        return itemsInBox > 0;
    }
}
