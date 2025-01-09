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
    }

    private void FixedUpdate()
    {
        if(startMovement)
        {
            t += Time.fixedDeltaTime * speed;

            if (t > 1f)
            {
                t = 1f;
                startMovement = false;
                ShipSceneManager.Instance.SetObjectsToSpawn();
                SceneManager.LoadScene(MapManager.Instance.GetCurrentLevel()._node.sceneName);
            }

            rb.MovePosition(CalculateQuadraticBezierPoint(t, points[0], points[1], points[2]));
            rb.MoveRotation(Quaternion.LookRotation(CalculateBezierTangent(t, points[0], points[1], points[2]),Vector3.up));
        }
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return (u * u * p0) + (2 * u * t * p1) + (t * t * p2);
    }

    private Vector3 CalculateBezierTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        Vector3 tangent = (2 * u * (p1 - p0)) + (2 * t * (p2 - p1));
        return tangent.normalized; 
    }

    public void SetStartMovement(bool state, List<Vector3> _points)
    {
        startMovement = state;
        points = _points;
    }
}
