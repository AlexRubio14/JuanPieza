using UnityEngine;

public class ObjectState : MonoBehaviour
{
    [Header("Sibling")]
    [SerializeField] private GameObject fixedSibling;
    [SerializeField] private GameObject brokenSibling;

    [SerializeField] private bool isBroken;

    public void SetIsBroke(bool state)
    {
        if(isBroken == state)
            return;

        isBroken = state;
        if (fixedSibling)
            fixedSibling.SetActive(!state);
        if(brokenSibling)
            brokenSibling.SetActive(state);
    }

    public bool GetIsBroken()
    {
        return isBroken;
    }
}
