using UnityEngine;

public class ObjectState : MonoBehaviour
{
    [Header("Sibling")]
    [SerializeField] private GameObject brokenSibling;
    [SerializeField] private GameObject fixedSibling;

    [SerializeField] private bool isBroken;

    public void SetIsBroke(bool state)
    {
        isBroken = state;
        brokenSibling.SetActive(state);
        fixedSibling.SetActive(!state);
    }

    public bool GetIsBroken()
    {
        return isBroken;
    }
}
