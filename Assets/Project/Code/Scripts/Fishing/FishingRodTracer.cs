using UnityEngine;

public class FishingRodTracer : WeaponTracer
{
    [SerializeField]
    private FishingRod fishingRod;

    [SerializeField]
    private bool displayTrace;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if(fishingRod.chargingHook)
            PredictTrajectory(0, fishingRod.throwForce);
        lineRenderer.enabled = fishingRod.chargingHook;
    }

    private void OnDrawGizmos()
    {
        if (displayTrace)
        {
            PredictTrajectory(0, fishingRod.throwForce);
        }
    }
}
