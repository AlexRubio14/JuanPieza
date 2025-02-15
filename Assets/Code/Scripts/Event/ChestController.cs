using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] private EventChest[] chests;

    public void DesactiveChests()
    {
        foreach (var chest in chests)
            chest.SetCanInteract(false);
    }
}
