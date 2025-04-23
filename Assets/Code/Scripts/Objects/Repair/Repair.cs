using UnityEngine;

public class Repair : InteractableObject
{
    [Space, Header("Repair Item")]
    [SerializeField] protected ObjectSO repairItem;
    [SerializeField] protected ObjectState state;
    [SerializeField] protected GameObject repairParticles;
    [SerializeField] protected AudioClip repairClip;

    protected float repairProgress;



    #region Object Fuctions
    public override void Grab(ObjectHolder _objectHolder) { }
    public override void Release(ObjectHolder _objectHolder) { }
    public override void Interact(ObjectHolder _objectHolder) { }
    public override void Use(ObjectHolder _objectHolder) { }
    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return handObject && handObject.objectSO == repairItem;
    }
    #endregion

    #region Repair Functions

    public void BreakIce()
    {
        if (TryGetComponent(out FreezeWeapon weapon))
        {
            weapon.BreakIce();
            return;
        }
    }

    public void RepairProgress(float _repairProgressed)
    {
        if (TryGetComponent(out Cannon cannon))
        {
            if(cannon.GetFreeze())
                return;
        }

        repairProgress += _repairProgressed;
        tooltip.progressBar.EnableProgressBar(true);
        tooltip.progressBar.SetProgress(repairProgress, 1);

        if (repairProgress >= 1)
        {
            RepairEnded();
        }
    }

    public ObjectSO GetRepairItem() { return repairItem; }
    public ObjectState GetObjectState() { return state; }
    public virtual void OnBreakObject() { }
    protected virtual void RepairEnded() 
    {
        tooltip.progressBar.EnableProgressBar(false);
        repairProgress = 0;
    }
    #endregion

}
