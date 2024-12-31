using UnityEngine;

public class RepairObject : Repair
{
    [Header("Sibling")]
    [SerializeField] private GameObject fixedSibling;
    protected override void RepairEnded(ObjectHolder _objectHolder)
    {
        fixedSibling.SetActive(true);
        gameObject.SetActive(false);
    }
}
