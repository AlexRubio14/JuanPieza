using UnityEngine;

public class WeaponTracer : MonoBehaviour
{

    [SerializeField]
    protected LayerMask hitLayers;
    [SerializeField]
    protected Rigidbody rb;
    [SerializeField]
    protected Transform starterPos;

    [Space, SerializeField]
    protected int maxSteps;
    [SerializeField]
    protected float stepSize;

    [SerializeField]
    protected LineRenderer lineRenderer;

    protected virtual void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    protected void PredictTrajectory(float _force)
    {
        Vector3 velocity = _force / rb.mass * starterPos.forward;
        Vector3 position = starterPos.position;
        UpdateLineRenderer(1, (0, position));
        for (int i = 1; i <= maxSteps; i++)
        {
            velocity = CalculateNewVelocity(velocity, rb.linearDamping, Time.fixedDeltaTime);
            Vector3 nextPosition = position + velocity * Time.fixedDeltaTime;

            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, 5f, hitLayers))
            {
                if(i != 0)
                    UpdateLineRenderer(i, (i - 1, hit.point));
                break;
            }

            position = nextPosition;

            UpdateLineRenderer(maxSteps, (i, position));
        }

        
    }

    protected Vector3 CalculateNewVelocity(Vector3 _velocity, float _drag, float _increment)
    {
        _velocity += Physics.gravity * _increment;
        _velocity *= Mathf.Clamp01(1f - (_drag * 1.1f) * _increment);
        return _velocity;
    }

    protected void UpdateLineRenderer(int _count, (int point, Vector3 pos) _pointPos)
    {
        if (_pointPos.point >= _count)
            return;

        lineRenderer.positionCount = _count;
        lineRenderer.SetPosition(_pointPos.point, _pointPos.pos);
    }

}
