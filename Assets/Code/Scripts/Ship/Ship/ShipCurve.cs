using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipCurve : AllyShip
{
    private List<Vector3> points;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float timeArrive;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;
    private float t;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool startMovementCurve = false;
    private bool startMovementToIsland = false;
    private float step;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        InitAllyBoat();
        VotationCanvasManager.Instance.SetVotationUIState(false);
    }

    private void FixedUpdate()
    {
        CurveBehaviour();
    }


    private void CurveBehaviour() 
    {
        if (startMovementCurve)
            MoveCurve();
        else if (startMovementToIsland)
            MoveToIsland();
    }
    private void MoveCurve()
    {
        t += Time.fixedDeltaTime * speed;

        rb.MovePosition(CalculateQuadraticBezierPoint(t, points[0], points[1], points[2]));

        if (t > 0.1f)
            FinishCurve();
    }

    private void ChangeScene()
    {
        switch (MapManager.Instance.GetCurrentLevel().nodeType)
        {
            case NodeData.NodeType.BATTLE:
                SceneManager.LoadScene("Battle");
                break;
            case NodeData.NodeType.EVENT:
                SceneManager.LoadScene("Event");
                break;
            case NodeData.NodeType.SHOP:
                SceneManager.LoadScene("Shop");
                break;
            case NodeData.NodeType.BOSS:
                SceneManager.LoadScene("Battle");
                break;
        }
    }

    private void FinishCurve()
    {
        MapManager.Instance.isVoting = false;
        startMovementCurve = false;
        ShipSceneManager.Instance.SetObjectsToSpawn();
        ShipSceneManager.Instance.SetShipId(idShip, currentHealth, targetHeight, isBarrelBoxActive);
        ChangeScene();
    }
    private void MoveToIsland()
    {
        step = (Vector3.Distance(startPosition, targetPosition) / timeArrive) * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);


        if (transform.position.z >= 0)
        {
            SetStartMovementToIsland(false);
            CameraManager.Instance.SetArriveCamera(false);
            CameraManager.Instance.SetSimpleCamera(true);
            ActiveBridge(true);
        }
    }


    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return (u * u * p0) + (2 * u * t * p1) + (t * t * p2);
    }

    public void SetStartMovementCurve(bool state, List<Vector3> _points)
    {
        startMovementCurve = state;
        points = _points;
    }

    public bool IsMoving()
    {
        return startMovementCurve;
    }

    public void SetStartMovementToIsland(bool state)
    {
        startMovementToIsland = state;
        if(state)
        {
            startPosition = transform.position;
            targetPosition = new Vector3(0, transform.position.y, 10);
        }
    }

    public void ActiveBridge(bool state)
    {
        transform.GetComponentInChildren<ShippingSail>().ActiveBridge(state);
    }

    public void SetIsMainShip(bool state)
    {
        ShippingSail sail = GetComponentInChildren<ShippingSail>();
        sail.SetIsMainShip(state);
    }

    public bool GetStartMovementToIsland()
    {
        return startMovementToIsland;
    }

    public float GetStep()
    {
        return step;
    }
}
