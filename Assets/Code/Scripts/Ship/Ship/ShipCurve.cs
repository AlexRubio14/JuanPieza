using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipCurve : AllyShip
{
    private List<Vector3> points;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float speedArriveIsland;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;
    private float t;
    private bool startMovementCurve = false;
    private bool startMovementToIsland = false;
    private bool rotateCamera = false;
    private Vector3 ingameCameraPos;
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
        else if (rotateCamera && !MapManager.Instance.GetIsVoting())
            SetIngameCamera();
    }
    private void MoveCurve()
    {
        t += Time.fixedDeltaTime * speed;

        rb.MovePosition(CalculateQuadraticBezierPoint(t, points[0], points[1], points[2]));

        if (t > 0.5f)
            FinishCurve();
    }
    private void FinishCurve()
    {
        MapManager.Instance.isVoting = false;
        startMovementCurve = false;
        ShipSceneManager.Instance.SetObjectsToSpawn();
        ShipSceneManager.Instance.SetShipId(idShip, currentHealth, targetHeight, isBarrelBoxActive);
        SceneManager.LoadScene(MapManager.Instance.GetCurrentLevel().sceneName);
    }
    private void MoveToIsland()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y, 10), Time.fixedDeltaTime * speedArriveIsland);

        if (transform.position.z >= 0)
        {
            rotateCamera = true;
            ingameCameraPos = transform.position;
            ingameCameraPos.y = 20;
            SetStartMovementToIsland(false);
            CameraManager.Instance.SetArriveCamera(false);
            CameraManager.Instance.SetSimpleCamera(true);
            ActiveBridge(true);
        }
    }
    private void SetIngameCamera()
    {
        Quaternion targetRotation = Quaternion.Euler(40, 0, 0);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        transform.position = Vector3.Lerp(transform.position, ingameCameraPos, Time.deltaTime * rotationSpeed);

        if (Quaternion.Angle(Camera.main.transform.rotation, targetRotation) < 0.1f)
            rotateCamera = false;
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

    public void SetStartMovementToIsland(bool state)
    {
        startMovementToIsland = state;
    }

    public void ActiveBridge(bool state)
    {
        transform.Find("Sail").GetComponentInChildren<ShippingSail>().ActiveBridge(state);
    }

    public void SetIsMainShip(bool state)
    {
        transform.Find("Sail").GetComponentInChildren<ShippingSail>().SetIsMainShip(state);
    }
}
