using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreResources : InteractableObject
{
    [Header("Store Resources")]
    [SerializeField] private ObjectSO itemBuy;
    [SerializeField] private int price;
    
    private ShipsManager shipsManager;

    private void Start()
    {
        shipsManager = ShipsManager.instance;
    }
    public override void Interact(ObjectHolder _objectHolder)
    {
        List<Box> boxes = shipsManager.playerShip.GetInventory().Select(obj => obj.GetComponent<Box>()).Where(box => box != null).ToList();

        foreach (Box box in boxes)
        {
            if (box.GetItemDrop() == itemBuy)
            {
                MoneyManager.Instance.SpendMoney(price);
                box.AddItemInBox();
            }
        }
    }

    public override void UseItem(ObjectHolder _objectHolder)
    {
        
    }
}
