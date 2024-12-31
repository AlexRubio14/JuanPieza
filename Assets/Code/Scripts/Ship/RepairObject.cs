using UnityEngine;

public class RepairObject : Repair
{
    [Header("Sibling")]
    [SerializeField] private GameObject fixedSibling;
    protected override void RepairEnded(PlayerController player)
    {
        fixedSibling.SetActive(true);
        gameObject.SetActive(false);
    }
}
