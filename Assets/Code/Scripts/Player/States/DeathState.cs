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
        if (hookPosition == Vector3.zero) //Si no hay ningun anzuelo en tu zona del mar 
            SeaDrag();
        else //Si hay algun anzuelo
        {
            //Si la distancia hacia el  anzuelo es menor a X esperar a ser rescatado
            if (Vector3.Distance(transform.position, endPosition) <= 1)
                WaitToGetRescued();
            else
                SwimToHook();
        }
    }
    public override void FixedUpdateState()
    {
           
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

    private void SeaDrag()
    {
        //Lerp de la posicion inicial a la final 
        lerpProcess += Time.deltaTime;
        rb.position = Vector3.Lerp(startPosition, endPosition, lerpProcess / controller.timeToDie);

        if (lerpProcess / controller.timeToDie >= 1)
            Debug.Log("Ha muerto");
        
    }

    private void WaitToGetRescued()
    {
        //Esperar a ser revivido
        Debug.Log("Esta cerca del anzuelo");
    }
    private void SwimToHook()
    {
        //Lerp de la posicion actual hacia la del anzuelo mas cercano
        rb.position = transform.position + (endPosition - transform.position).normalized * controller.swimSpeed * Time.deltaTime;
        controller.SetRotation((endPosition - transform.position).normalized);
        //Si mientras se esta yendo se quita el anzuelo se reiniciara el lerp hacia la muerte y la posicion inicial se volvera la actual
    }
}
