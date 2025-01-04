using UnityEngine;

public abstract class Weapon : InteractableObject
{
    public bool hasAmmo { get; private set; }
    [SerializeField] GameObject ammoObject;

    [SerializeField] protected Transform ridingPos;

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

        PlayerController player = _objectHolder.transform.parent.gameObject.GetComponent<PlayerController>();

        PlayersManager.instance.players[player.playerInput.playerReference].Item1.SwitchCurrentActionMap("CannonGameplay");
        
        if(hasAmmo) 
        {

        }

        //si tiene bala

        //si no tiene bala
    }

    protected void Shoot()
    {

    }

    public override void UseItem(ObjectHolder _objectHolder)
    {

    }
}
