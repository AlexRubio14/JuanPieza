using UnityEngine;

public class HandWeaponTracer : WeaponTracer
{
    protected Weapon weapon;

    protected override void Awake()
    {
        base.Awake();
        weapon = GetComponentInParent<Weapon>();
    }

    void FixedUpdate()
    {
        if (weapon.isBeginUsed && !weapon.isTilting)
            PredictTrajectory(weapon.bulletForce, Vector2.zero);

        lineRenderer.enabled = weapon.isBeginUsed && !weapon.isTilting;
    }
}
