using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private GameObject itemDropped;

    public void DropItem(PlayerController player)
    {
        if(player.item == null)
        {
            //Change stata
            GameObject item = Instantiate(itemDropped);
            item.transform.SetParent(player.transform, true);

            //Colocarlo delante del player y resetar su rotación

            player.SetItem(item);
        }
    }
}
