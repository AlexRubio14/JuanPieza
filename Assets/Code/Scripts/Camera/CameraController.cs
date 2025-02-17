using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private enum CameraMovement { NONE, ZOOM_IN, ZOOM_OUT };

    [SerializeField]
    private CameraMovement camState = CameraMovement.NONE;

    [Header("Players"), SerializeField]
    private List<Collider> followColliders;
    private List<PlayerStateMachine> playerStates = new List<PlayerStateMachine>();

    [Header("Position Adjustments"), SerializeField]
    private float minYDistance;
    [SerializeField]
    private float maxZDistance;
    private float zPosition;
    [SerializeField]
    private float zOffset;
    [SerializeField]
    private float deadPlayerDistance;

    [Space, Header("Cameras"), SerializeField]
    private Camera insideCamera;
    [SerializeField]
    private Camera externalCamera;

    [Header("Cameras Variables"), SerializeField, Range(0, 1)]
    private float movementSpeed;
    [SerializeField]
    private float zoomOutSpeed;
    [SerializeField]
    private float zoomInSpeed;
    [SerializeField]
    private float XZSpeed;
    
    private void Awake()
    {
        playerStates = new List<PlayerStateMachine>();

        //Guardamos la Y del primer Player
        //Seteamos todos los players con la misma posicion en Y
        foreach (Collider item in followColliders)
            playerStates.Add(item.GetComponent<PlayerStateMachine>());
        
        
        //colocar la camara a la distancia minima
        zPosition = transform.position.z - GetMiddlePointBetweenPlayers().z;
    }

    private void Start()
    {
        //if (ShipsManager.instance.playerShip.TryGetComponent(out Collider playerShipColl))
        //    AddObject(playerShipColl.gameObject);

        //foreach (Ship ship in ShipsManager.instance.enemiesShips)
        //{
        //    if (ship != null)
        //    {
        //        if (ship.TryGetComponent(out Collider enemyShipCol))
        //            AddObject(enemyShipCol.gameObject);
        //    }
        //}
    }

    public void AddPlayer(GameObject _newPlayer)
    {
        followColliders.Add(_newPlayer.GetComponent<CapsuleCollider>());
        playerStates.Add(_newPlayer.GetComponent<PlayerStateMachine>());
    }

    public void AddObject(GameObject _objectToAdd)
    {
        followColliders.Add(_objectToAdd.GetComponent<Collider>());
        playerStates.Add(null);

    }

    public void RemovePlayer(GameObject _removablePlayer)
    {
        CapsuleCollider currentCollider = _removablePlayer.GetComponent<CapsuleCollider>();
        int collisionIndex = followColliders.IndexOf(currentCollider);
        followColliders.RemoveAt(collisionIndex);
        playerStates.RemoveAt(collisionIndex);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CheckCamDistance();
        MoveCamera();
    }

    private void CheckCamDistance()
    {
        bool zoomIn = true;
        bool zoomOut = false;
        List<Collider> activePlayers = new List<Collider>();
        for (int i = 0; i < followColliders.Count; i++)
        {
            if (followColliders[i] != null && playerStates != null && (playerStates[i] == null || playerStates[i].currentState != playerStates[i].deathState))
            {
                activePlayers.Add(followColliders[i]);

            }
        }

        foreach (Collider item in activePlayers)
        {
            //Obtenemos los planos que utiliza el fustrum de la camara externa
            Plane[] camFrustrum = GeometryUtility.CalculateFrustumPlanes(externalCamera);

            //Debug.DrawLine(item.bounds.min, item.bounds.max);

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
            (middlePos.z + zPosition / 2) - transform.position.z
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
            zPosition = Mathf.Clamp(zPosition - zoomSpeed * Time.fixedDeltaTime, -maxZDistance, maxZDistance);
        }

        Vector3 finalPos = Vector3.Lerp
            (
            transform.position,
            destinyPos,
            movementSpeed * Time.fixedDeltaTime
            );
        finalPos.y = Mathf.Clamp(finalPos.y, minYDistance, Mathf.Infinity);
        finalPos.z -= zOffset;

        transform.position = finalPos;
    }
    private Vector3 GetMiddlePointBetweenPlayers()
    {
        Vector3 middlePoint = Vector3.zero;

        for (int i = 0; i < followColliders.Count; i++)
        {
            if (followColliders[i] != null && playerStates.Count > 0 
                && (playerStates[i] == null || playerStates[i].currentState != playerStates[i].deathState || playerStates[i].transform.position.z >= deadPlayerDistance))
            {
                middlePoint += followColliders[i].transform.position;
            }
        }

        if (middlePoint == Vector3.zero)
            return Vector3.zero;

        middlePoint /= followColliders.Count;

        return middlePoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(GetMiddlePointBetweenPlayers(),0.4f);
    }
    
}
