using UnityEngine;

public class Hammer : Tool, ICatapultAmmo
{
    [Space, Header("Hammer"), SerializeField]
    private float hitTime;
    [SerializeField]
    private float hitCD;
    [SerializeField]
    private float hitRadius;
    [SerializeField]
    private float repairAmmount;
    [SerializeField]
    private LayerMask hitLayers;
    private bool canHit = true;

    [Space, SerializeField] 
    private Transform hitPosition;
    [SerializeField]
    private GameObject hitParticles;
    [SerializeField]
    private AudioClip hitClip;

    private PlayerController playerCont;

    public override void Use(ObjectHolder _objectHolder)
    {
        if (!canHit)
            return;

        canHit = false;
        
        //Hay que cambiarlo por hittear
        playerCont = _objectHolder.GetComponentInParent<PlayerController>();
        //Playear Anim de Golpear
        playerCont.animator.SetTrigger("HitHammer");

        //Invoke una funcion 
        //Esta funcion tiene que suceder a la vez del frame donde inpacta 
        Invoke("HitHammer", hitTime);
        Invoke("HammerCD", hitCD);
        
        //InteractableObject nearObject = _objectHolder.GetNearestInteractableObject();

        //if (!nearObject)
        //    return;

        //if(nearObject is RepairObject)
        //    (nearObject as RepairObject).AddPlayer(_objectHolder);
        //else if (nearObject is RepairHole)
        //    (nearObject as RepairHole).AddPlayer(_objectHolder);
    }


    private void HitHammer()
    {
        if (!isBeingUsed)
            return;

        //Esta funcion ha de hacer
        //SphereCast en frente del player que compruebe: Objetos / Players / Enemigos 

        RaycastHit[] hits = Physics.SphereCastAll(hitPosition.position, hitRadius, playerCont.transform.forward, 1, hitLayers);

        foreach (RaycastHit hit in hits)
        {
            if(hit.collider.TryGetComponent(out Repair _repair))
            {
                //Objetos: Comprobar si son objetos rompibles y si estan rotos llamar a la funcion de RepairProgress
                if(_repair.GetObjectState().GetIsBroken())
                    _repair.RepairProgress(repairAmmount);
            }
            else if(hit.collider.TryGetComponent(out PlayerController _hittedPlayer))
            {
                if(_hittedPlayer != playerCont)
                {
                    //Players: Llamar a una funcion del player controler para stunearlo
                    _hittedPlayer.stateMachine.ChangeState(_hittedPlayer.stateMachine.stunedState);
                }
            }
            else if (hit.collider.TryGetComponent(out PirateBoardingController _enemy))
            {
                //Enemigos: Stunear a los enemigos

            }
        }

        //Poner particulas
        Instantiate(hitParticles, hitPosition.position, Quaternion.identity);
        //Hacer sonido
        AudioManager.instance.Play2dOneShotSound(hitClip, "Objects", 1, 0.95f, 1.05f);
    }
    private void HammerCD()
    {
        canHit = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPosition.position, hitRadius);
    }
}
