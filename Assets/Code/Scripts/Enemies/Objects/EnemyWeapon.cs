using UnityEngine;

public abstract class EnemyWeapon : EnemyObject
{
    protected bool isLoaded = false;
    [field: SerializeField]
    public Transform weaponShootPosition {  get; protected set; }

    private void Start()
    {
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
    }
    public virtual void LoadWeapon()
    {
        isLoaded = true;
        enemieManager.AddShootCannonAction(this);
    }
}
