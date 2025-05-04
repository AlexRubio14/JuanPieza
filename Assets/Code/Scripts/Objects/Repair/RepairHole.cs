using UnityEngine;

public class RepairHole : Repair
{
    private Ship ship;
    private float damageDeal;
    [SerializeField] private float hpPercentage;
    [SerializeField] private bool hasToRecoverHP;

    protected override void RepairEnded()
    {
        base.RepairEnded();
        if(hasToRecoverHP)
            ship.SetCurrentHealth(damageDeal * hpPercentage);
        Destroy(gameObject);
    }
    public void SetbulletInformation(Ship _ship, float amount)
    {
        ship = _ship;
        damageDeal = amount;
    }
    public void SetDamageDeal(float _damageDeal) 
    {
        damageDeal -= _damageDeal;
    }
    public float GetDamageDeal()
    {
        return damageDeal;
    }
    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return false;
    }
}
