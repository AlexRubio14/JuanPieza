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

    public override void UseItem(ObjectHolder _objectHolder)
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
}
