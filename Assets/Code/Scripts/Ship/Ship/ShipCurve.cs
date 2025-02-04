using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipCurve : AllyShip
{
    private List<Vector3> points;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float speedArriveIsland;

    private Rigidbody rb;
    private float t;
    private bool startMovementCurve;
    private bool startMovementToIsland;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        base.InitAllyBoat();
        VotationCanvasManager.Instance.SetVotationUIState(false);
    }

    private void FixedUpdate()
    {
        if(startMovementCurve)
        {
            t += Time.fixedDeltaTime * speed;

            if (t > 0.1f)
            {
                MapManager.Instance.isVoting = false;
                startMovementCurve = false;
                ShipSceneManager.Instance.SetObjectsToSpawn();
                ShipSceneManager.Instance.SetShipId(idShip, currentHealth, targetHeight, isBarrelBoxActive);
                SceneManager.LoadScene(MapManager.Instance.GetCurrentLevel().sceneName);
            }

            rb.MovePosition(CalculateQuadraticBezierPoint(t, points[0], points[1], points[2]));
        }
        if(startMovementToIsland)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y, 10), Time.fixedDeltaTime * speedArriveIsland);

            if (transform.position.z >= 0)
            {
                SetStartMovementToIsland(false);
                CameraManager.Instance.SetArriveCamera(false);
                CameraManager.Instance.SetSimpleCamera(true);
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

    public void SetStartMovementToIsland(bool state)
    {
        startMovementToIsland = state;
    }
}
