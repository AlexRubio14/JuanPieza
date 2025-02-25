using UnityEngine;

public class Cigarrette : Resource
{
    [Space, SerializeField] private float smokeTime;
    private float currentTime;
    private bool isSmoking;

    protected override void Start()
    {
        base.Start();
        isSmoking = false;
        currentTime = smokeTime;
    }
    private void Update()
    {
        if (isSmoking)
        {
            //particulillas
            currentTime -= Time.deltaTime;
            if (currentTime <= 0) 
            {
                //cancelar animacion player
            }
        }
    }


    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        return !_objectHolder.GetHasObjectPicked();
    }

    public override HintController.ActionType[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        if (!_objectHolder.GetHasObjectPicked())
            return new HintController.ActionType[] { HintController.ActionType.INTERACT, HintController.ActionType.CANT_USE};
        else if (_objectHolder.GetHandInteractableObject() == this)
            return new HintController.ActionType[] { HintController.ActionType.INTERACT, HintController.ActionType.USE };

        return new HintController.ActionType[] { HintController.ActionType.NONE };
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        base.Interact(_objectHolder);
        _objectHolder.hintController.UpdateActionType(new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.INTERACT, "drop"),
            new HintController.Hint(HintController.ActionType.USE, "smoke")
        });
    }

    public override void Use(ObjectHolder _objectHolder)
    {
        //playear animacion player
        _objectHolder.GetComponentInParent<CigarretteController>().ActivateCigarrette();
        _objectHolder.RemoveItemFromHand();
        _objectHolder.GetComponentInParent<PlayerController>().animator.SetBool("Pick", false);

        _objectHolder.hintController.UpdateActionType(new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        });

        Destroy(gameObject);
        
    }
}
