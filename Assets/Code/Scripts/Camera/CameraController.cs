using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private enum CameraMovement { NONE, ZOOM_IN, ZOOM_OUT };

    [SerializeField]
    private CameraMovement camState = CameraMovement.NONE;

    [Header("Players"), SerializeField]
    private List<Collider> playerColliders;
    private List<PlayerStateMachine> playerStates = new List<PlayerStateMachine>();

    [Header("Players Variables"), SerializeField]
    private float minYDistance;
    [SerializeField]
    private float maxZDistance;
    private float zOffset;

    [Space, Header("Cameras"), SerializeField]
    private Camera insideCamera;
    [SerializeField]
    private Camera externalCamera;
    private bool defaultCamera;

    [Header("Cameras Variables"), SerializeField, Range(0, 1)]
    private float movementSpeed;
    [SerializeField]
    private float zoomOutSpeed;
    [SerializeField]
    private float zoomInSpeed;
    [SerializeField]
    private float XZSpeed;
    
    private void Start()
    {
        playerStates = new List<PlayerStateMachine>();
        defaultCamera = true;

        //Guardamos la Y del primer Player
        //Seteamos todos los players con la misma posicion en Y
        foreach (Collider item in playerColliders)
        {
            playerStates.Add(item.GetComponent<PlayerStateMachine>());
        }
        
        //colocar la camara a la distancia minima
        zOffset = transform.position.z - GetMiddlePointBetweenPlayers().z;
    }

    public void AddPlayer(GameObject _newPlayer)
    {
        playerColliders.Add(_newPlayer.GetComponent<CapsuleCollider>());
        playerStates.Add(_newPlayer.GetComponent<PlayerStateMachine>());
    }
    public void RemovePlayer(GameObject _removablePlayer)
    {
        playerColliders.Remove(_removablePlayer.GetComponent<CapsuleCollider>());
        playerStates.Remove(_removablePlayer.GetComponent<PlayerStateMachine>());
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (defaultCamera)
        {
            CheckCamDistance();
            MoveCamera();
        }
    }

    private void CheckCamDistance()
    {
        bool zoomIn = true;
        bool zoomOut = false;
        List<Collider> activePlayers = new List<Collider>();
        for (int i = 0; i < playerColliders.Count; i++)
        {
            if (playerColliders[i] != null && playerStates != null && (playerStates[i] == null || playerStates[i].currentState != playerStates[i].deathState))
            {
                activePlayers.Add(playerColliders[i]);

            }
        }

        foreach (Collider item in activePlayers)
        {
            //Obtenemos los planos que utiliza el fustrum de la camara externa
            Plane[] camFrustrum = GeometryUtility.CalculateFrustumPlanes(externalCamera);
            
            //Comprobamos si el player esta dentro del frustrum
            if (GeometryUtility.TestPlanesAABB(camFrustrum, item.bounds)) //Si esta dentro de la camara
            {
                //Obtenemos los planos que utiliza el fustrum de la camara interna
                camFrustrum = GeometryUtility.CalculateFrustumPlanes(insideCamera);

                if (!GeometryUtility.TestPlanesAABB(camFrustrum, item.bounds)) //Si hay algun player fuera de la camara interna NO hacer ZOOM_IN 
                    zoomIn = false;

            }
            else //Si esta fuera de la camara exterior alejamos la cam                
                zoomOut = true;

        }

        if (zoomOut)
            camState = CameraMovement.ZOOM_OUT;
        else if(zoomIn)
            camState = CameraMovement.ZOOM_IN;
        else
            camState = CameraMovement.NONE;

    }

    private void MoveCamera() 
    {
        Vector3 destinyPos = transform.position;

        Vector3 middlePos = GetMiddlePointBetweenPlayers();

        if (middlePos == Vector3.zero)
            return;

        Vector3 XZDir = new Vector3
            (
            middlePos.x - transform.position.x,
            0,
            (middlePos.z + zOffset / 2) - transform.position.z
            );
        destinyPos += XZDir * XZSpeed;

        if (camState != CameraMovement.NONE)
        {
            float zoomSpeed = 1;
            switch (camState)
            {
                case CameraMovement.ZOOM_IN:
                    zoomSpeed = -zoomInSpeed;
                    break;
                case CameraMovement.ZOOM_OUT:
                    zoomSpeed = zoomOutSpeed;
                    break;
                default:
                    break;
            }
            destinyPos += -transform.forward * zoomSpeed;
            zOffset = Mathf.Clamp(zOffset - zoomSpeed * Time.fixedDeltaTime, -maxZDistance, maxZDistance);
        }

        Vector3 finalPos = Vector3.Lerp
            (
            transform.position,
            destinyPos,
            movementSpeed * Time.fixedDeltaTime
            );
        finalPos.y = Mathf.Clamp(finalPos.y, minYDistance, Mathf.Infinity);

        transform.position = finalPos;
        
    }
    private Vector3 GetMiddlePointBetweenPlayers()
    {
        Vector3 middlePoint = Vector3.zero;

        for (int i = 0; i < playerColliders.Count; i++)
        {
            if (playerColliders[i] != null && playerStates.Count > 0 && (playerStates[i] == null || playerStates[i].currentState != playerStates[i].deathState))
            {
                middlePoint += playerColliders[i].transform.position;

            }
        }

        if (middlePoint == Vector3.zero)
            return Vector3.zero;

        middlePoint /= playerColliders.Count;
        return middlePoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(GetMiddlePointBetweenPlayers(),0.4f);
    }

    public void SetDefaultCamera(bool value)
    {
        defaultCamera = value;
    }
    
}
