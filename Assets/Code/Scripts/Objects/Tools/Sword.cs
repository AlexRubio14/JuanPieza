using UnityEngine;

public class Sword : Tool
{
    [SerializeField] protected CapsuleCollider attackCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        base.Interact(_objectHolder);
    }

    public override void Use(ObjectHolder _objectHolder)
    {
        Debug.Log("Corte con la espada");
        //empezar funcion de ataque
    }

    public void ActivateAttackCollider()
    {
        attackCollider.enabled = true;
    }

    public void DeactivateAttackCollider()
    {
        attackCollider.enabled = false;
    }

    public override HintController.Hint[] ShowNeededInputHint(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();
        if (handObject && handObject == this)
        {
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "drop"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "attack")
            };
        }
        else if (!handObject)
        {
            return new HintController.Hint[]
            {
                new HintController.Hint(HintController.ActionType.INTERACT, "grab"),
                new HintController.Hint(HintController.ActionType.CANT_USE, "")
            };
        }

        return new HintController.Hint[]
        {
            new HintController.Hint(HintController.ActionType.NONE, "")
        };

    }
}
