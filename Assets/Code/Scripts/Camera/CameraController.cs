using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private enum CameraMovement { NONE, ZOOM_IN, ZOOM_OUT };

    [SerializeField]
    private CameraMovement camState = CameraMovement.NONE;

    [Serializable]    
    private struct ObjectFollow
    {
        public ObjectFollow(Collider _collider, float _weight, PlayerStateMachine _player)
        {
            collider = _collider;
            weight = _weight;
            player = _player;
        }

        public Collider collider;
        public float weight;
        public PlayerStateMachine player;
    }

    [Header("Objects to Follow"), SerializeField]
    private List<ObjectFollow> followObjects;
    private int totalPlayers;
    private int totalEnemies;

    [Header("Position Adjustments"), SerializeField]
    private float minYDistance;
    [SerializeField]
    private float maxZDistance;
    private float zPosition;
    [SerializeField]
    private float zOffset;
    [SerializeField]
    private float playerWeight;
    [SerializeField]
    private float enemyWeight;
    [SerializeField]
    private float objectWeight;


    [Space, Header("Cameras"), SerializeField]
    private Camera insideCamera;
    [SerializeField]
    private Camera externalCamera;

    [Header("Cameras Variables"), SerializeField, Range(0, 1)]
    private float movementSpeed;
    [SerializeField]
    private float maxMovementSpeed;
    [SerializeField]
    private float slowingDistance;
    [SerializeField]
    private float zoomOutSpeed;
    [SerializeField]
    private float zoomInSpeed;
    [SerializeField]
    private float XZSpeed;
    
    private void Awake()
    {

        //Guardamos la Y del primer Player
        //Seteamos todos los players con la misma posicion en Y
        for (int i = 0; i < followObjects.Count; i++)
        {
            ObjectFollow follow = followObjects[i];
            follow.player = followObjects[i].collider.GetComponent<PlayerStateMachine>();
            followObjects[i] = follow;
        }

        
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
        totalPlayers++;

        for (int i = 0; i < followObjects.Count; i++)
        {
            if (followObjects[i].player != null)
            {
                ObjectFollow updatedPlayer = followObjects[i];
                updatedPlayer.weight = playerWeight / totalPlayers;
                followObjects[i] = updatedPlayer;
            }
        }

        followObjects.Add(new ObjectFollow(
            _newPlayer.GetComponent<CapsuleCollider>(),
            playerWeight / totalPlayers,
            _newPlayer.GetComponent<PlayerStateMachine>()
            ));
    }
    public void AddBounds(GameObject _objectToAdd)
    {
        followObjects.Add(new ObjectFollow(
            _objectToAdd.GetComponent<Collider>(),
            objectWeight,
            null
            ));

    }

    public void RemovePlayer(GameObject _removablePlayer)
    {
        totalPlayers--;
        CapsuleCollider currentCollider = _removablePlayer.GetComponent<CapsuleCollider>();
        ObjectFollow playerToRemove = GetObjectByCollider(currentCollider);
        if(playerToRemove.collider != null)
        followObjects.Remove(playerToRemove);
    }

    private ObjectFollow GetObjectByCollider(Collider _collider)
    {
        foreach (ObjectFollow item in followObjects)
        {
            if(item.collider == _collider)
                return item;
        }

        return new ObjectFollow(null, 0, null);
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
        for (int i = 0; i < followObjects.Count; i++)
        {
            if (followObjects[i].collider != null && (followObjects[i].player == null || followObjects[i].player.currentState != followObjects[i].player.deathState))
                activePlayers.Add(followObjects[i].collider);

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


        destinyPos.y = Mathf.Clamp(destinyPos.y, minYDistance, Mathf.Infinity);
        destinyPos.z -= zOffset;
        Vector3 finalPos = Vector3.zero;

        if (Vector3.Distance(transform.position, destinyPos) > slowingDistance)
            finalPos = transform.position + (destinyPos - transform.position).normalized * maxMovementSpeed * Time.fixedDeltaTime;
        else
            finalPos = Vector3.Lerp(transform.position, destinyPos, movementSpeed * Time.fixedDeltaTime);

        transform.position = finalPos;
    }
    private Vector3 GetMiddlePointBetweenPlayers()
    {
        Vector3 middlePoint = Vector3.zero;

        for (int i = 0; i < followObjects.Count; i++)
        {
            if (followObjects[i].collider != null && followObjects.Count > 0 
                && (followObjects[i].player == null || followObjects[i].player.currentState != followObjects[i].player.deathState))
            {
                middlePoint += followObjects[i].collider.transform.position * followObjects[i].weight;
            }
        }

        if (middlePoint == Vector3.zero)
            return Vector3.zero;

        middlePoint /= followObjects.Count;

        return middlePoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(GetMiddlePointBetweenPlayers(),0.4f);
    }
    
}
