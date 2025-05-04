using System.Collections;
using UnityEngine;

public class ObjectState : MonoBehaviour
{
    [Header("Sibling")]
    [SerializeField] private GameObject fixedSibling;
    [SerializeField] private GameObject brokenSibling;

    [SerializeField] private bool isBroken;
    [SerializeField] private AudioClip brokeClip;

    private bool oneHit;
    private bool timeBroken;

    private void Start()
    {
        SetIsBroke(isBroken);
    }

    public void SetIsBroke(bool state, bool freezing = false)
    {
        isBroken = state;

        if (freezing)
            return;

        StartCoroutine(SetBrokenStateDelayed(state, 2f));

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

    private IEnumerator SetBrokenStateDelayed(bool state, float delay)
    {
        yield return new WaitForSeconds(delay);
        timeBroken = state;
    }

    public bool GetIsBroken()
    {
        return isBroken;
    }

    public bool GetTimeBroken()
    {
        return timeBroken;
    }

    public void SetOneHit(bool state)
    {
        oneHit = state;
    }
}
