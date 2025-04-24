using UnityEngine;

public class Repair : InteractableObject
{
    [Space, Header("Repair Item")]
    [SerializeField] protected ObjectSO repairItem;
    [SerializeField] protected ObjectState state;
    [SerializeField] protected GameObject repairParticles;
    [SerializeField] protected AudioClip repairClip;

    protected float repairProgress;

    protected virtual void Start()
    {
        if (state.GetIsBroken())
            OnBreakObject();
    }

    #region Object Fuctions
    public override void Grab(ObjectHolder _objectHolder) { }
    public override void Release(ObjectHolder _objectHolder) { }
    public override void Interact(ObjectHolder _objectHolder) { }
    public override void Use(ObjectHolder _objectHolder) { }
    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return false;
    }
    public override bool CanGrab(ObjectHolder _objectHolder)
    {
        return base.CanGrab(_objectHolder) && !state.GetIsBroken(); 
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
