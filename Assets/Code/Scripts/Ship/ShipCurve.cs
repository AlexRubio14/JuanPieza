using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ShipCurve : MonoBehaviour
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
            }

            Vector3 targetPosition = CalculateQuadraticBezierPoint(t, points[0], points[1], points[2]);

            rb.MovePosition(targetPosition);
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
