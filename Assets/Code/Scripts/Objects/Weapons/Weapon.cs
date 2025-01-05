using UnityEngine;

public abstract class Weapon : InteractableObject
{
    [Space, Header("Weapon"), SerializeField]
    protected Transform ridingPos;
    public bool hasAmmo { get; private set; }

    protected Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();

        hasAmmo = false;
    }

    public override void Interact(ObjectHolder _objectHolder)
    {
        if (!CanInteract(_objectHolder))
            return;

        PlayerController player = _objectHolder.transform.parent.gameObject.GetComponent<PlayerController>();

        if (hasAmmo) //Si tienes municion
        {
            //Cambiar el mapa de inputs
            PlayersManager.instance.players[player.playerInput.playerReference].Item1.SwitchCurrentActionMap("CannonGameplay");
            //Cambiar estado del player
            PlayerStateMachine playerSM = _objectHolder.GetComponentInParent<PlayerStateMachine>();
            playerSM.ChangeState(playerSM.cannonState);
            //Hacer Tp al player a la posicion de montar al cañon
            player.transform.position = ridingPos.position;
            //El padre del arma sera el player 
            transform.SetParent(player.transform);
        }
        else //Si no tiene municion agregar la bala al cañon
        {
            //Settear a true la municion
            hasAmmo = true;
            //Borrar la bala de la mano
            InteractableObject currentObject = _objectHolder.RemoveItemFromHand();
            Destroy(currentObject);
        }
    }
    public override void UseItem(ObjectHolder _objectHolder)
    {
        Shoot();
    }
    public override bool CanInteract(ObjectHolder _objectHolder)
    {
        InteractableObject handObject = _objectHolder.GetHandInteractableObject();

        return !handObject && hasAmmo || handObject && handObject.objectSO == objectToInteract;
    }

    protected abstract void Shoot();
}
