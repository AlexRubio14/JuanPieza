using UnityEngine;

public class ObjectState : MonoBehaviour
{
    [Header("Sibling")]
    [SerializeField] private GameObject fixedSibling;
    [SerializeField] private GameObject brokenSibling;

    [SerializeField] private bool isBroken;
    [SerializeField] private AudioClip brokeClip;

    private bool oneHit;

    private void Start()
    {
        SetIsBroke(isBroken);
    }

    public void SetIsBroke(bool state, bool freezing = false)
    {
        isBroken = state;

        if (freezing)
            return;

        if (fixedSibling)
            fixedSibling.SetActive(!state);
        if(brokenSibling)
            brokenSibling.SetActive(state);

        if(state)
        {
            AudioManager.instance.Play2dOneShotSound(brokeClip, "Objects");
            if (oneHit)
                Destroy(gameObject);
        }
    }

    public bool GetIsBroken()
    {
        return isBroken;
    }

    public void SetOneHit(bool state)
    {
        oneHit = state;
    }
}
