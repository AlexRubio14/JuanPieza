using UnityEngine;

public abstract class Weapon : InteractableObject
{
    public bool hasAmmo { get; private set; }
    [SerializeField] GameObject ammoObject;

    [SerializeField] protected Vector3 ridingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        //si tiene bala

        //si no tiene bala
    }

    protected void Shoot()
    {

    }

    public override void UseItem(ObjectHolder _objectHolder)
    {
        throw new System.NotImplementedException();
    }
}
