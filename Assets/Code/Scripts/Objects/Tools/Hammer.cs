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
    [SerializeField]
    private AudioClip hitObjectClip;
    [SerializeField]
    private AudioClip hitIceClip;
    [SerializeField]
    private AudioClip hitPlayerClip;

    [SerializeField]
    private float stunnedTime;

    [Space, Header("Rumble"), SerializeField]
    private RumbleController.RumblePressets hitRumble;
    [SerializeField]
    private RumbleController.RumblePressets playerStunRumble;

    private PlayerController playerCont;

    public override void Use(ObjectHolder _objectHolder)
    {
        if (!canHit)
            return;

        canHit = false;
        
        //Hay que cambiarlo por hittear
        playerCont = _objectHolder.playerController;
        //Playear Anim de Golpear
        playerCont.animator.SetTrigger("HitHammer");


        //Invoke una funcion 
        //Esta funcion tiene que suceder a la vez del frame donde inpacta 
        Invoke("HitHammer", hitTime);
        Invoke("HammerCD", hitCD);
        
    }


    private void HitHammer()
    {
        if (!isBeginUsed)
            return;


        //Esta funcion ha de hacer
        //SphereCast en frente del player que compruebe: Objetos / Players / Enemigos 
        RaycastHit[] hits = Physics.SphereCastAll(hitPosition.position, hitRadius, playerCont.transform.forward, 1, hitLayers);

        bool somethingHitted = false;

        foreach (RaycastHit hit in hits)
        {
            if(hit.collider.TryGetComponent(out Repair _repair))
            {
                //Objetos: Comprobar si son objetos rompibles y si estan rotos llamar a la funcion de RepairProgress
                if (_repair.GetObjectState().GetIsBroken())
                {
                    _repair.RepairProgress(repairAmmount);
                    AudioManager.instance.Play2dOneShotSound(hitObjectClip, "Objects", 0.1f, 0.8f, 1.2f);
                }

                if ((_repair is Weapon))
                    _repair.BreakIce();
                somethingHitted = true;
            }
            else if(hit.collider.TryGetComponent(out PlayerController _hittedPlayer))
            {
                if(_hittedPlayer != playerCont)
                {
                    //Players: Llamar a una funcion del player controler para stunearlo
                    _hittedPlayer.stateMachine.stunedState.stunedRumble = playerStunRumble;
                    _hittedPlayer.stateMachine.stunedState.maxTimeStunned = stunnedTime;
                    _hittedPlayer.stateMachine.ChangeState(_hittedPlayer.stateMachine.stunedState);
                    AudioManager.instance.Play2dOneShotSound(hitPlayerClip, "Objects", 1, 0.9f, 1.1f);
                    somethingHitted = true;
                }
            }
            else if (hit.collider.TryGetComponent(out PirateBoardingController _enemy))
            {
                //Enemigos: Stunear a los enemigos
                somethingHitted = true;
            }
        }

        //Poner particulas
        Instantiate(hitParticles, hitPosition.position, Quaternion.identity);
        //Hacer sonido
        AudioManager.instance.Play2dOneShotSound(hitClip, "Objects", 0.4f, 0.75f, 1.25f);
        if(somethingHitted)
            PlayersManager.instance.players[playerCont.playerInput.playerReference].rumbleController.AddRumble(hitRumble);

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
