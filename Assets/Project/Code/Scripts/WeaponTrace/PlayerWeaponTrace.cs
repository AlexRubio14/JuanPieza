using UnityEngine;

public class PlayerWeaponTrace : WeaponTracer
{
    protected Weapon weapon;

    protected override void Awake()
    {
        base.Awake();
        weapon = GetComponentInParent<Weapon>();
    }

    void FixedUpdate()
    {
        if (weapon.isTilting)
            PredictTrajectory(weapon.bulletForce, Vector2.zero);

        lineRenderer.enabled = weapon.isTilting;
        decal.SetActive(weapon.isTilting);
        if (decal.activeInHierarchy)
            decal.transform.forward = -collisionNormal;
    }
}
