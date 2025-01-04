public abstract class EnemyWeapon : EnemyObject
{
    public override void OnBreakObject()
    {
        enemieManager.AddRepairCannonAction(gameObject);
    }

    public override void OnFixObject()
    {

    }
}
