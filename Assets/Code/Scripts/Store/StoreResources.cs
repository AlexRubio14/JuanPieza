using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreResources : Box
{
    private ShipsManager shipsManager;

    protected override void Start()
    {
        base.Start();
        shipsManager = ShipsManager.instance;
    }
    public override void Interact(ObjectHolder _objectHolder)
    {
        List<Box> boxes = shipsManager.playerShip.GetInventory().Select(obj => obj.GetComponent<Box>()).Where(box => box != null).ToList();

        foreach (Box box in boxes)
        {
            if (box.GetItemDrop() == itemDropped)
            {
                if (MoneyManager.Instance.SpendMoney(itemDropped.price))
                {
                    box.AddItemInBox();
                }
                return;
            }
        }
    }

    public override void UseItem(ObjectHolder _objectHolder)
    {
        
    }
}
