using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] private EventChest[] chests;

    public void DesactiveChests()
    {
        foreach (var chest in chests)
            chest.SetCanInteract(false);
    }

    public void ActiveChest(bool state)
    {
        foreach (var chest in chests)
            chest.gameObject.SetActive(state);
    }
}
