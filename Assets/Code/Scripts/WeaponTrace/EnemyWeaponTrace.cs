using UnityEngine;

public class EnemyWeaponTrace : WeaponTracer
{
    [SerializeField]
    protected EnemyWeapon weapon;

    [SerializeField]
    private bool displayTrace;
    protected override void Awake()
    {
        base.Awake();
        weapon = GetComponentInParent<EnemyWeapon>();
    }

    private void FixedUpdate()
    {
        if(weapon.isLoaded)
            PredictTrajectory(weapon.bulletForce, Vector2.zero);

        lineRenderer.enabled = weapon.isLoaded;
        decal.SetActive(weapon.isLoaded);
        if (decal.activeInHierarchy)
            decal.transform.forward = -collisionNormal;
    }

    private void OnDrawGizmosSelected()
    {
        if (displayTrace)
            PredictTrajectory(weapon.bulletForce, Vector2.zero);
    }
}
