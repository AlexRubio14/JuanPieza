using UnityEngine;

public class DeathState : PlayerState
{

    private Vector3 startPosition;
    private Vector3 endPosition;

    private float lerpProcess;
    public Vector3 hookPosition {  get; private set; }

    public Transform transform => controller.transform;
    private Rigidbody rb => controller.GetRB();

    public override void EnterState()
    {
        rb.isKinematic = true;
        CalculateDeathPos();
        //Spawnear señal de ayuda

        hookPosition = Vector3.zero;
        FishingManager.instance.AddDeadPlayer(this);

    }
    public override void UpdateState()
    {
    }
    public override void FixedUpdateState()
    {
        
        
        if(hookPosition == Vector3.zero) //Si no hay ningun anzuelo en tu zona del mar 
        {
            //Lerp de la posicion inicial a la final 
            lerpProcess += Time.fixedDeltaTime;
            rb.position = Vector3.Lerp(startPosition, endPosition, lerpProcess / controller.timeToDie);

            if (lerpProcess / controller.timeToDie >= 1)
                Debug.Log("Ha muerto");

        }
        else //Si hay algun anzuelo
        {
            //Lerp de la posicion actual hacia la del anzuelo mas cercano
            rb.position = Vector3.Lerp(startPosition, endPosition, Time.fixedDeltaTime * controller.swimSpeed);

            //Cuando se llegue a la posicion del anzuelo activar evento de recoger    
            if(Vector3.Distance(startPosition, endPosition) <= 1)
            {
                //Revivir
                Debug.Log("Revive");
            }
            //Si mientras se esta yendo se quita el anzuelo se reiniciara el lerp hacia la muerte y la posicion inicial se volvera la actual
        }
           
    }
    public override void ExitState()
    {
        FishingManager.instance.RemoveDeadPlayer(this);
        rb.isKinematic = false;
    }

    public override void RollAction() { /*No puedes rodar*/ }

    public override void InteractAction() { /*No puedes interactuar*/ }

    public override void UseAction() { /*No puedes usar ningun objeto*/ }

    public override void OnCollisionEnter(Collision collision)
    {
    }

    public void CalculateDeathPos()
    {
        lerpProcess = 0;
        hookPosition = Vector3.zero;

        startPosition = transform.position;
        startPosition.y = FishingManager.instance.defaultYPos;

        endPosition.x = transform.position.x;
        endPosition.y = FishingManager.instance.defaultYPos;
        endPosition.z = FishingManager.instance.deathZPos;
    }

    public void SetHookPosition(Vector3 _hookPos)
    {
        lerpProcess = 0;
        Vector3 finalHookPos = _hookPos;
        finalHookPos.y = FishingManager.instance.defaultYPos;
        hookPosition = finalHookPos;

        startPosition = transform.position;
        startPosition.y = FishingManager.instance.defaultYPos;

        endPosition = hookPosition;
    }

}
