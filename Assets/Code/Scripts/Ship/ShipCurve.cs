using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipCurve : Ship
{
    private List<Vector3> points;

    [Header("Movement")]
    [SerializeField] private float speed;

    private Rigidbody rb;
    private float t;
    private bool startMovement;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Initialize();
        VotationCanvasManager.Instance.SetVotationUIState(false);
    }

    private void FixedUpdate()
    {
        if(startMovement)
        {
            t += Time.fixedDeltaTime * speed;

            if (t > 0.5f)
            {
                MapManager.Instance.isVoting = false;
                startMovement = false;
                ShipSceneManager.Instance.SetObjectsToSpawn();
                ShipSceneManager.Instance.SetShipId(idShip, currentHealth, targetHeight, isBarrelBoxActive);
                ShipSceneManager.Instance.SetPlayerPosition();
                SceneManager.LoadScene(MapManager.Instance.GetCurrentLevel().sceneName);
            }

            rb.MovePosition(CalculateQuadraticBezierPoint(t, points[0], points[1], points[2]));
        }
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return (u * u * p0) + (2 * u * t * p1) + (t * t * p2);
    }

    public void SetStartMovement(bool state, List<Vector3> _points)
    {
        startMovement = state;
        points = _points;
    }
}
