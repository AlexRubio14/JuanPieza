using UnityEngine;

public abstract class Weapon : InteractableObject
{
    [Space, Header("Weapon"), SerializeField]
    protected Transform ridingPos;
    public bool hasAmmo { get; private set; }

    public override void Interact(ObjectHolder _objectHolder)
    {
        
        PlayerController player = _objectHolder.transform.parent.gameObject.GetComponent<PlayerController>();

        if (hasAmmo) //Si tienes municion
        {
            //Cambiar el mapa de inputs
            PlayersManager.instance.players[player.playerInput.playerReference].Item1.SwitchCurrentActionMap("CannonGameplay");
            player.transform.position = ridingPos.position;

            transform.SetParent(player.transform);
        }
        else //Si no tiene municion 
        {
            if ( _objectHolder.GetHandInteractableObject().objectSO == objectToInteract) //Comprobar si tiene el objeto necesario en la mano en la mano
            {

            }
            
        }
    }

    protected abstract void Shoot();

    public override void UseItem(ObjectHolder _objectHolder)
    {
        Shoot();
    }
}
