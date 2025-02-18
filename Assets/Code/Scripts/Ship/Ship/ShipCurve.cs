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
    private bool rotateCamenra = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        base.InitAllyBoat();
        VotationCanvasManager.Instance.SetVotationUIState(false);
    }

    private void FixedUpdate()
    {
        MoveCurve();
        MoveToIsland();
    }

    private void MoveCurve()
    {
        if (startMovementCurve)
        {
            t += Time.fixedDeltaTime * speed;

            if (t > 0.1f)
            {
                MapManager.Instance.isVoting = false;
                startMovementCurve = false;
                ShipSceneManager.Instance.SetObjectsToSpawn();
                ShipSceneManager.Instance.SetShipId(idShip, currentHealth, targetHeight, isBarrelBoxActive);
                ChangeScene();
            }

            rb.MovePosition(CalculateQuadraticBezierPoint(t, points[0], points[1], points[2]));
        }
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

    private void MoveToIsland()
    {
        if (startMovementToIsland)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y, 10), Time.fixedDeltaTime * speedArriveIsland);

            if (transform.position.z >= 0)
            {
                rotateCamenra = true;
                SetStartMovementToIsland(false);
                CameraManager.Instance.SetArriveCamera(false);
                CameraManager.Instance.SetSimpleCamera(true);
                ActiveBridge(true);
            }

        }
        if (rotateCamenra && !MapManager.Instance.GetIsVoting())
        {
            Quaternion targetRotation = Quaternion.Euler(40, 0, 0);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            if (Quaternion.Angle(Camera.main.transform.rotation, targetRotation) < 0.1f)
            {
                rotateCamenra = false;
            }

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
