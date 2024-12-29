using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private InteractableObject itemDropped;
    private int itemsInBox;

    public InteractableObject DropItem(PlayerController player)
    {
        if(player.item == null && HasItem())
        {
            RemoveItemInBox();
            return itemDropped;
        }

        return null;
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
