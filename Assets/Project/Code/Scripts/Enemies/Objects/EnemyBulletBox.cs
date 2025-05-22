public class EnemyBulletBox : EnemyObject
{
    public override void OnBreakObject() { enemieManager.AddRepairBulletSpawnAction(); }

    public override void OnFixObject() { }

    public override void UseObject() { }
}
