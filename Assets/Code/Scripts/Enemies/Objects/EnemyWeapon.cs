using UnityEngine;
public abstract class EnemyWeapon : EnemyObject
{
    public bool isLoaded {  get; private set; }

    [field: SerializeField]
    public Transform shooterPosition {  get; protected set; }
    [field: SerializeField]
    public Transform bulletSpawnPosition {  get; protected set; }

    [SerializeField]
    protected GameObject bullet;

    [field: SerializeField]
    public float weaponDamage { get; protected set; }

    [field: SerializeField]
    public float bulletForce { get; protected set; }
    [field: SerializeField]
    public float shootHeightOffset { get; protected set; }
    [field: SerializeField]
    public float aimSpeed { get; protected set; }


    private void Start()
    {
        isLoaded = false;
        enemieManager.AddReloadCannonAction(this);
    }

    public override void OnBreakObject()
    {
        enemieManager.AddRepairCannonAction(this);
        isLoaded = false;
    }
    public override void OnFixObject()
    {
        enemieManager.AddReloadCannonAction(this);
    }

    public override void UseObject()
    {
        enemieManager.AddReloadCannonAction(this);
        isLoaded = false;
    }
    public virtual void LoadWeapon()
    {
        isLoaded = true;
        enemieManager.AddShootCannonAction(this);
    }
}
