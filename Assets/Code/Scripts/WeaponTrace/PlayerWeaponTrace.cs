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
        if (weapon.isPlayerMounted())
            PredictTrajectory(weapon.bulletForce);

        lineRenderer.enabled = weapon.isPlayerMounted();
        decal.SetActive(weapon.isPlayerMounted());
    }
}
