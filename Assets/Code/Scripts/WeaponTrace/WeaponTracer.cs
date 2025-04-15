using Unity.VisualScripting;
using UnityEngine;

public class WeaponTracer : MonoBehaviour
{
    [SerializeField]
    protected GameObject decalPrefab;
    protected GameObject decal;
    [Space, SerializeField]
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

    protected Vector3 collisionNormal;
    protected virtual void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if(decalPrefab)
            decal = Instantiate(decalPrefab);
    }

    protected void PredictTrajectory(float _force, Vector2 _forceVector)
    {

        Vector3 velocity;
        if (_force != 0)
            velocity = _force / rb.mass * starterPos.forward;
        else
            velocity = starterPos.forward * (_forceVector.x / rb.mass) + Vector3.up * (_forceVector.y / rb.mass);

        Vector3 position = starterPos.position;
        
        UpdateLineRenderer(1, (0, position));
        for (int i = 1; i <= maxSteps; i++)
        {
            velocity = CalculateNewVelocity(velocity, rb.linearDamping, Time.fixedDeltaTime);
            Vector3 nextPosition = position + velocity * Time.fixedDeltaTime;

            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, stepSize, hitLayers))
            {
                if(i != 1)
                    UpdateLineRenderer(i, (i - 1, hit.point));
                
                if(decal != null)
                    decal.transform.position = hit.point;
                collisionNormal = hit.normal;
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
