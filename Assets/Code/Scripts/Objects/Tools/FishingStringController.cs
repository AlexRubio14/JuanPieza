using UnityEngine;

public class FishingStringController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private FishingRod fishingRod;

    [SerializeField]
    private Transform startStringPos;
    private void Awake()
    {
        fishingRod = GetComponentInParent<FishingRod>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, startStringPos.position);
        lineRenderer.SetPosition(1, fishingRod.hook.hookStringPos.position);
    }
}
