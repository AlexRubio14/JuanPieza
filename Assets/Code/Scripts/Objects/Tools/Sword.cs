using UnityEditor;
using UnityEngine;

public class Sword : Tool
{
    [SerializeField] protected CapsuleCollider attackCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
